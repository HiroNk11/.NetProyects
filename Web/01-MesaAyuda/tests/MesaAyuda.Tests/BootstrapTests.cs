using MesaAyuda.Web.Data;
using MesaAyuda.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MesaAyuda.Tests;

public class BootstrapTests : IClassFixture<HelpdeskFactory>
{
    private readonly HelpdeskFactory factory;
    public BootstrapTests(HelpdeskFactory factory) => this.factory = factory;

    private static IConfiguration Settings(params (string Key, string Value)[] values) =>
        new ConfigurationBuilder().AddInMemoryCollection(values.ToDictionary(x => "Bootstrap:" + x.Key, x => (string?)x.Value)).Build();

    [Fact]
    public async Task Creates_private_accounts_without_resetting_password_on_restart()
    {
        await using var scope = factory.Services.CreateAsyncScope();
        var config = Settings(("Enabled", "true"), ("AdminEmail", "private-admin@example.test"), ("AdminPassword", "Private.Test2026!"),
            ("TechnicianEmail", "private-tech@example.test"), ("TechnicianPassword", "Private.Tech2026!"),
            ("RequesterEmail", "private-user@example.test"), ("RequesterPassword", "Private.User2026!"));
        await BootstrapAccounts.InitializeAsync(scope.ServiceProvider, config);
        var users = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var admin = (await users.FindByEmailAsync("private-admin@example.test"))!;
        Assert.True(await users.IsInRoleAsync(admin, Roles.Admin));
        Assert.True(await users.CheckPasswordAsync(admin, "Private.Test2026!"));
        Assert.True(await users.IsInRoleAsync((await users.FindByEmailAsync("private-tech@example.test"))!, Roles.Technician));
        Assert.True(await users.IsInRoleAsync((await users.FindByEmailAsync("private-user@example.test"))!, Roles.Requester));
        await BootstrapAccounts.InitializeAsync(scope.ServiceProvider, Settings(("Enabled", "true"),
            ("AdminEmail", admin.Email!), ("AdminPassword", "Different.Password2026!")));
        Assert.True(await users.CheckPasswordAsync(admin, "Private.Test2026!"));
        Assert.False(await users.CheckPasswordAsync(admin, "Different.Password2026!"));
    }

    [Fact]
    public async Task Disabled_bootstrap_does_not_create_accounts()
    {
        await using var scope = factory.Services.CreateAsyncScope();
        await BootstrapAccounts.InitializeAsync(scope.ServiceProvider, Settings(("AdminEmail", "disabled@example.test")));
        Assert.Null(await scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>().FindByEmailAsync("disabled@example.test"));
    }

    [Fact]
    public async Task Existing_requester_cannot_be_promoted_by_bootstrap()
    {
        await using var scope = factory.Services.CreateAsyncScope();
        await Assert.ThrowsAsync<InvalidOperationException>(() => BootstrapAccounts.InitializeAsync(scope.ServiceProvider,
            Settings(("Enabled", "true"), ("AdminEmail", "solicitante@mesa.local"), ("AdminPassword", "Private.Test2026!"))));
        var users = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        Assert.False(await users.IsInRoleAsync((await users.FindByEmailAsync("solicitante@mesa.local"))!, Roles.Admin));
    }

    [Fact]
    public async Task Weak_optional_password_rolls_back_all_new_accounts()
    {
        await using (var scope = factory.Services.CreateAsyncScope())
            await Assert.ThrowsAsync<InvalidOperationException>(() => BootstrapAccounts.InitializeAsync(scope.ServiceProvider,
                Settings(("Enabled", "true"), ("AdminEmail", "rollback@example.test"), ("AdminPassword", "Private.Test2026!"),
                    ("TechnicianEmail", "weak@example.test"), ("TechnicianPassword", "weak"))));
        await using var verification = factory.Services.CreateAsyncScope();
        var users = verification.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        Assert.Null(await users.FindByEmailAsync("rollback@example.test"));
        Assert.Null(await users.FindByEmailAsync("weak@example.test"));
    }

    [Theory]
    [InlineData("", "Private.Test2026!", "")]
    [InlineData("invalid", "Private.Test2026!", "")]
    [InlineData("valid@example.test", "", "")]
    [InlineData("valid@example.test", "Private.Test2026!", "partial@example.test")]
    public async Task Invalid_configuration_fails_before_writing(string email, string password, string technician)
    {
        await using var scope = factory.Services.CreateAsyncScope();
        await Assert.ThrowsAsync<InvalidOperationException>(() => BootstrapAccounts.InitializeAsync(scope.ServiceProvider,
            Settings(("Enabled", "true"), ("AdminEmail", email), ("AdminPassword", password), ("TechnicianEmail", technician))));
    }

    [Fact]
    public void SqlServer_has_its_own_current_migrations()
    {
        using var db = new SqlServerHelpdeskDesignFactory().CreateDbContext([]);
        Assert.False(db.Database.HasPendingModelChanges());
        var script = db.GetService<IMigrator>().GenerateScript(options: MigrationsSqlGenerationOptions.Idempotent);
        Assert.Contains("IDENTITY", script);
        Assert.Contains("nvarchar", script);
        Assert.Contains("CREATE TABLE [Tickets]", script);
        Assert.DoesNotContain("InitialHelpdesk", script);
    }
}
