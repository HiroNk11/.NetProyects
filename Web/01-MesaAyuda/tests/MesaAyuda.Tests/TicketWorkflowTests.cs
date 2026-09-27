using System.Net;
using System.Security.Claims;
using MesaAyuda.Web.Data;
using MesaAyuda.Web.Models;
using MesaAyuda.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MesaAyuda.Tests;

public class TicketWorkflowTests(HelpdeskFactory factory) : IClassFixture<HelpdeskFactory>
{
    private async Task<int> Create(HttpClient client, string? title = null)
    {
        var response = await HelpdeskFactory.PostAsync(client, "/Tickets/Create", "/Tickets/Create",
            ("Title", title ?? $"Solicitud {Guid.NewGuid():N}"), ("Description", "Detalle suficiente para reproducir el problema."),
            ("Priority", "Alta"));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        return int.Parse(response.Headers.Location!.ToString().Split('/').Last());
    }
    private async Task<Ticket> Read(int id)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        return await scope.ServiceProvider.GetRequiredService<HelpdeskDbContext>().Tickets
            .AsNoTracking().Include(t => t.Events).Include(t => t.Comments).SingleAsync(t => t.Id == id);
    }
    private async Task<string> TechnicianId()
    {
        await using var scope = factory.Services.CreateAsyncScope();
        return (await scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>()
            .FindByEmailAsync("tecnico@mesa.local"))!.Id;
    }
    private async Task<HttpResponseMessage> Status(HttpClient client, int id, string status, int version) =>
        await HelpdeskFactory.PostAsync(client, $"/Tickets/Details/{id}", $"/Tickets/Status/{id}",
            ("Status", status), ("Version", version.ToString()));
    private async Task Assign(HttpClient admin, int id, int version)
    {
        var response = await HelpdeskFactory.PostAsync(admin, $"/Tickets/Details/{id}", $"/Tickets/Assign/{id}",
            ("TechnicianId", await TechnicianId()), ("Version", version.ToString()));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }

    [Fact]
    public async Task Anonymous_requests_require_login()
    {
        using var client = factory.Anonymous();
        var response = await client.GetAsync("/Tickets");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Account/Login", response.Headers.Location!.ToString());
    }
    [Fact]
    public async Task Login_does_not_redirect_to_external_sites()
    {
        using var client = factory.Anonymous();
        var response = await HelpdeskFactory.PostAsync(client, "/Account/Login", "/Account/Login",
            ("Email", "admin@mesa.local"), ("Password", DemoData.Password), ("ReturnUrl", "https://example.org"));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.DoesNotContain("example.org", response.Headers.Location!.ToString());
    }
    [Fact]
    public async Task Logout_requires_post_and_invalidates_session()
    {
        using var client = await factory.LoginAsync("solicitante");
        var response = await HelpdeskFactory.PostAsync(client, "/Tickets", "/Account/Logout");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/Tickets")).StatusCode);
    }
    [Fact]
    public async Task Creation_persists_owner_and_audit_and_ignores_overposted_state()
    {
        using var client = await factory.LoginAsync("solicitante");
        var response = await HelpdeskFactory.PostAsync(client, "/Tickets/Create", "/Tickets/Create",
            ("Title", "Prueba de creación"), ("Description", "Una descripción válida de la solicitud."),
            ("Priority", "Media"), ("Status", "Cerrado"), ("RequesterId", "otra-persona"));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var ticket = await Read(int.Parse(response.Headers.Location!.ToString().Split('/').Last()));
        Assert.Equal(TicketStatus.Nuevo, ticket.Status);
        Assert.NotEqual("otra-persona", ticket.RequesterId);
        Assert.Single(ticket.Events);
        Assert.Equal(ticket.RequesterId, ticket.Events[0].ActorId);
    }
    [Fact]
    public async Task Invalid_creation_and_missing_antiforgery_are_rejected()
    {
        using var client = await factory.LoginAsync("solicitante");
        var invalid = await HelpdeskFactory.PostAsync(client, "/Tickets/Create", "/Tickets/Create",
            ("Title", "x"), ("Description", "x"), ("Priority", "999"));
        Assert.Equal(HttpStatusCode.OK, invalid.StatusCode);
        Assert.Contains("validation-summary-errors", await invalid.Content.ReadAsStringAsync());
        var csrf = await client.PostAsync("/Tickets/Create", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Title"] = "Ticket sin token", ["Description"] = "Descripción suficiente para probar."
        }));
        Assert.Equal(HttpStatusCode.BadRequest, csrf.StatusCode);
    }
    [Fact]
    public async Task Requester_and_unassigned_technician_cannot_read_someone_elses_ticket()
    {
        using var admin = await factory.LoginAsync("admin");
        using var requester = await factory.LoginAsync("solicitante");
        using var technician = await factory.LoginAsync("tecnico");
        var id = await Create(admin, "Caso confidencial de administración");
        Assert.Equal(HttpStatusCode.NotFound, (await requester.GetAsync($"/Tickets/Details/{id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await technician.GetAsync($"/Tickets/Details/{id}")).StatusCode);
        var list = await requester.GetStringAsync("/Tickets?Query=confidencial");
        Assert.DoesNotContain("Caso confidencial", list);
        var comment = await HelpdeskFactory.PostAsync(requester, "/Tickets", $"/Tickets/Comment/{id}",
            ("Body", "No debería guardarse."), ("Version", "1"));
        Assert.Equal(HttpStatusCode.NotFound, comment.StatusCode);
        Assert.Empty((await Read(id)).Comments);
    }
    [Fact]
    public async Task Requester_cannot_assign_or_change_status()
    {
        using var requester = await factory.LoginAsync("solicitante");
        var id = await Create(requester);
        var assign = await HelpdeskFactory.PostAsync(requester, $"/Tickets/Details/{id}", $"/Tickets/Assign/{id}",
            ("TechnicianId", await TechnicianId()), ("Version", "1"));
        Assert.Equal(HttpStatusCode.Redirect, assign.StatusCode);
        Assert.Contains("/Account/Denied", assign.Headers.Location!.ToString());
        var status = await Status(requester, id, "EnCurso", 1);
        Assert.Equal(HttpStatusCode.Redirect, status.StatusCode);
        Assert.Contains("/Account/Denied", status.Headers.Location!.ToString());
        Assert.Equal(1, (await Read(id)).Version);
    }
    [Fact]
    public async Task Complete_lifecycle_supports_reopening_and_prevents_changes_after_closure()
    {
        using var requester = await factory.LoginAsync("solicitante");
        using var admin = await factory.LoginAsync("admin");
        using var technician = await factory.LoginAsync("tecnico");
        var id = await Create(requester);
        Assert.Equal(HttpStatusCode.BadRequest, (await Status(admin, id, "EnCurso", 1)).StatusCode);
        await Assign(admin, id, 1);
        Assert.Equal(HttpStatusCode.BadRequest, (await Status(technician, id, "Cerrado", 2)).StatusCode);
        var version = 2;
        foreach (var next in new[] { "EnCurso", "Resuelto", "EnCurso", "Resuelto", "Cerrado" })
            Assert.Equal(HttpStatusCode.Redirect, (await Status(technician, id, next, version++)).StatusCode);
        var closed = await Read(id);
        Assert.Equal(TicketStatus.Cerrado, closed.Status);
        Assert.Equal(7, closed.Events.Count);
        Assert.Equal(HttpStatusCode.BadRequest, (await Status(admin, id, "EnCurso", closed.Version)).StatusCode);
        var comment = await HelpdeskFactory.PostAsync(requester, $"/Tickets/Details/{id}", $"/Tickets/Comment/{id}",
            ("Body", "No debe agregarse al caso cerrado."), ("Version", closed.Version.ToString()));
        Assert.Equal(HttpStatusCode.BadRequest, comment.StatusCode);
        Assert.Empty((await Read(id)).Comments);
    }
    [Fact]
    public async Task Comments_are_persisted_and_encoded_and_stale_edits_return_conflict()
    {
        using var client = await factory.LoginAsync("solicitante");
        var id = await Create(client);
        const string body = "<script>alert('test')</script>";
        var response = await HelpdeskFactory.PostAsync(client, $"/Tickets/Details/{id}", $"/Tickets/Comment/{id}",
            ("Body", body), ("Version", "1"));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var html = await client.GetStringAsync($"/Tickets/Details/{id}");
        Assert.DoesNotContain(body, html);
        Assert.Contains("&lt;script&gt;", html);
        var stale = await HelpdeskFactory.PostAsync(client, $"/Tickets/Details/{id}", $"/Tickets/Comment/{id}",
            ("Body", "Edición obsoleta"), ("Version", "1"));
        Assert.Equal(HttpStatusCode.Conflict, stale.StatusCode);
        var ticket = await Read(id);
        Assert.Single(ticket.Comments);
        Assert.Equal(body, ticket.Comments[0].Body);
        Assert.Equal(2, ticket.Events.Count);
    }
    [Fact]
    public async Task Database_concurrency_token_rolls_back_comment_and_audit()
    {
        using var client = await factory.LoginAsync("solicitante");
        var id = await Create(client);
        await using var first = factory.Services.CreateAsyncScope();
        await using var second = factory.Services.CreateAsyncScope();
        var db1 = first.ServiceProvider.GetRequiredService<HelpdeskDbContext>();
        var db2 = second.ServiceProvider.GetRequiredService<HelpdeskDbContext>();
        var ticket1 = await db1.Tickets.SingleAsync(t => t.Id == id);
        await db2.Tickets.SingleAsync(t => t.Id == id);
        var actor = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, ticket1.RequesterId), new Claim(ClaimTypes.Role, Roles.Requester)], "test"));
        var winner = await first.ServiceProvider.GetRequiredService<TicketService>()
            .CommentAsync(id, new() { Body = "Primera edición", Version = 1 }, actor);
        var loser = await second.ServiceProvider.GetRequiredService<TicketService>()
            .CommentAsync(id, new() { Body = "Edición simultánea", Version = 1 }, actor);
        Assert.Equal(ChangeResult.Success, winner);
        Assert.Equal(ChangeResult.Conflict, loser);
        var saved = await Read(id);
        Assert.Single(saved.Comments);
        Assert.Equal("Primera edición", saved.Comments[0].Body);
        Assert.Equal(2, saved.Events.Count);
    }
    [Fact]
    public async Task Filters_and_pagination_keep_results_scoped()
    {
        using var client = await factory.LoginAsync("solicitante");
        var tag = $"Lote{Guid.NewGuid():N}";
        for (var i = 0; i < 11; i++) await Create(client, $"{tag} caso {i:D2}");
        var first = WebUtility.HtmlDecode(await client.GetStringAsync($"/Tickets?Query={tag}&Priority=Alta&Status=Nuevo"));
        Assert.Contains("Página 1 de 2", first);
        Assert.Contains($"{tag} caso 10", first);
        Assert.DoesNotContain($"{tag} caso 00", first);
        var second = WebUtility.HtmlDecode(await client.GetStringAsync($"/Tickets?Query={tag}&Priority=Alta&Status=Nuevo&Page=2"));
        Assert.Contains($"{tag} caso 00", second);
        var empty = WebUtility.HtmlDecode(await client.GetStringAsync($"/Tickets?Query={tag}&Status=Cerrado"));
        Assert.Contains("No hay tickets", empty);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.GetAsync("/Tickets?Page=0")).StatusCode);
    }
}
