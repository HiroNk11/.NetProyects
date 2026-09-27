using System.Security.Claims;
using MesaAyuda.Web.Data;
using MesaAyuda.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace MesaAyuda.Web.Services;

public enum ChangeResult { Success, NotFound, Forbidden, Invalid, Conflict }
public class TicketService(HelpdeskDbContext db, UserManager<AppUser> users, TimeProvider clock)
{
    private static string UserId(ClaimsPrincipal actor) =>
        actor.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();

    public IQueryable<Ticket> VisibleTo(ClaimsPrincipal actor)
    {
        var id = UserId(actor);
        if (actor.IsInRole(Roles.Admin)) return db.Tickets;
        if (actor.IsInRole(Roles.Technician))
            return db.Tickets.Where(t => t.TechnicianId == id || t.RequesterId == id);
        return db.Tickets.Where(t => t.RequesterId == id);
    }
    public bool CanManage(Ticket ticket, ClaimsPrincipal actor) =>
        actor.IsInRole(Roles.Admin) ||
        (actor.IsInRole(Roles.Technician) && ticket.TechnicianId == UserId(actor));

    public static bool CanTransition(TicketStatus from, TicketStatus to) => (from, to) switch
    {
        (TicketStatus.Nuevo, TicketStatus.EnCurso) => true,
        (TicketStatus.EnCurso, TicketStatus.Resuelto) => true,
        (TicketStatus.Resuelto, TicketStatus.EnCurso) => true,
        (TicketStatus.Resuelto, TicketStatus.Cerrado) => true,
        _ => false
    };
    public async Task<int> CreateAsync(CreateTicketInput input, ClaimsPrincipal actor)
    {
        if (string.IsNullOrWhiteSpace(input.Title) || input.Title.Trim().Length is < 5 or > 120 ||
            string.IsNullOrWhiteSpace(input.Description) || input.Description.Trim().Length is < 10 or > 4000 ||
            !Enum.IsDefined(input.Priority))
            throw new ArgumentException("Los datos del ticket no son válidos.");
        var ticket = new Ticket
        {
            Title = input.Title.Trim(), Description = input.Description.Trim(),
            Priority = input.Priority, RequesterId = UserId(actor),
            CreatedAt = clock.GetUtcNow().UtcDateTime, UpdatedAt = clock.GetUtcNow().UtcDateTime
        };
        ticket.Events.Add(Event(actor, "Ticket creado"));
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();
        return ticket.Id;
    }
    public async Task<ChangeResult> AssignAsync(int id, AssignInput input, ClaimsPrincipal actor)
    {
        if (!actor.IsInRole(Roles.Admin)) return ChangeResult.Forbidden;
        var ticket = await db.Tickets.FindAsync(id);
        if (ticket is null) return ChangeResult.NotFound;
        if (ticket.Version != input.Version) return ChangeResult.Conflict;
        if (ticket.Status == TicketStatus.Cerrado) return ChangeResult.Invalid;
        var technician = await users.FindByIdAsync(input.TechnicianId);
        if (technician is null || !await users.IsInRoleAsync(technician, Roles.Technician) ||
            ticket.TechnicianId == technician.Id) return ChangeResult.Invalid;
        ticket.TechnicianId = technician.Id;
        return await SaveAsync(ticket, actor, $"Asignado a {technician.DisplayName}");
    }
    public async Task<ChangeResult> ChangeStatusAsync(int id, ChangeStatusInput input, ClaimsPrincipal actor)
    {
        var ticket = await VisibleTo(actor).SingleOrDefaultAsync(t => t.Id == id);
        if (ticket is null) return ChangeResult.NotFound;
        if (!CanManage(ticket, actor)) return ChangeResult.Forbidden;
        if (ticket.Version != input.Version) return ChangeResult.Conflict;
        if (!CanTransition(ticket.Status, input.Status) ||
            (input.Status == TicketStatus.EnCurso && ticket.TechnicianId is null)) return ChangeResult.Invalid;
        var previous = ticket.Status;
        ticket.Status = input.Status;
        return await SaveAsync(ticket, actor, $"Estado: {previous} → {input.Status}");
    }
    public async Task<ChangeResult> CommentAsync(int id, CommentInput input, ClaimsPrincipal actor)
    {
        var ticket = await VisibleTo(actor).SingleOrDefaultAsync(t => t.Id == id);
        if (ticket is null) return ChangeResult.NotFound;
        if (ticket.Version != input.Version) return ChangeResult.Conflict;
        if (ticket.Status == TicketStatus.Cerrado || string.IsNullOrWhiteSpace(input.Body) ||
            input.Body.Trim().Length > 2000) return ChangeResult.Invalid;
        ticket.Comments.Add(new TicketComment
        {
            AuthorId = UserId(actor), Body = input.Body.Trim(), CreatedAt = clock.GetUtcNow().UtcDateTime
        });
        return await SaveAsync(ticket, actor, "Comentario agregado");
    }
    private TicketEvent Event(ClaimsPrincipal actor, string description) => new()
    {
        ActorId = UserId(actor), Description = description, CreatedAt = clock.GetUtcNow().UtcDateTime
    };
    private async Task<ChangeResult> SaveAsync(Ticket ticket, ClaimsPrincipal actor, string description)
    {
        ticket.Version++;
        ticket.UpdatedAt = clock.GetUtcNow().UtcDateTime;
        ticket.Events.Add(Event(actor, description));
        try
        {
            // El ticket, el comentario y la auditoría se guardan en una sola transacción.
            await db.SaveChangesAsync();
            return ChangeResult.Success;
        }
        catch (DbUpdateConcurrencyException)
        {
            db.ChangeTracker.Clear();
            return ChangeResult.Conflict;
        }
    }
}
