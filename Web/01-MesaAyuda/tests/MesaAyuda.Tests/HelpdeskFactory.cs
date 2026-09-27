using System.Net;
using System.Text.RegularExpressions;
using MesaAyuda.Web.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;

namespace MesaAyuda.Tests;

public class HelpdeskFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string database = Path.Combine(Path.GetTempPath(), $"mesa-tests-{Guid.NewGuid():N}.db");
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureLogging(logging => logging.ClearProviders().AddConsole());
        builder.ConfigureServices(services => services.AddDataProtection().UseEphemeralDataProtectionProvider());
        builder.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(
            new Dictionary<string, string?> { ["ConnectionStrings:Helpdesk"] = $"Data Source={database}" }));
    }
    public async Task InitializeAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<HelpdeskDbContext>();
        Assert.Equal(database, db.Database.GetDbConnection().DataSource);
        await db.Database.MigrateAsync();
        await DemoData.InitializeAsync(scope.ServiceProvider);
    }
    public HttpClient Anonymous() => CreateClient(new WebApplicationFactoryClientOptions
    {
        AllowAutoRedirect = false, BaseAddress = new Uri("https://localhost")
    });
    public async Task<HttpClient> LoginAsync(string role)
    {
        var client = Anonymous();
        var response = await PostAsync(client, "/Account/Login", "/Account/Login",
            ("Email", $"{role}@mesa.local"), ("Password", DemoData.Password));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        return client;
    }
    public static async Task<HttpResponseMessage> PostAsync(HttpClient client, string formPage, string target,
        params (string Key, string Value)[] fields)
    {
        var page = await client.GetAsync(formPage);
        page.EnsureSuccessStatusCode();
        var html = await page.Content.ReadAsStringAsync();
        var token = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"");
        Assert.True(token.Success, "El formulario debe incluir protección CSRF.");
        var data = fields.ToDictionary(x => x.Key, x => x.Value);
        data["__RequestVerificationToken"] = WebUtility.HtmlDecode(token.Groups[1].Value);
        return await client.PostAsync(target, new FormUrlEncodedContent(data));
    }
    async Task IAsyncLifetime.DisposeAsync()
    {
        await DisposeAsync();
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
        File.Delete(database);
        foreach (var suffix in new[] { "-wal", "-shm" })
            if (File.Exists(database + suffix)) File.Delete(database + suffix);
    }
}
