using System.ComponentModel.DataAnnotations;
using MesaAyuda.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace MesaAyuda.Web.Data;

public static class BootstrapAccounts
{
    public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("Bootstrap:Enabled")) return;
        var accounts = new List<(string Email, string Password, string Role)>();
        foreach (var (key, role) in new[] { ("Admin", Roles.Admin), ("Technician", Roles.Technician), ("Requester", Roles.Requester) })
        {
            var email = configuration[$"Bootstrap:{key}Email"]?.Trim();
            var password = configuration[$"Bootstrap:{key}Password"];
            if (key != "Admin" && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password)) continue;
            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email) || string.IsNullOrEmpty(password))
                throw new InvalidOperationException($"Bootstrap:{key} requiere correo válido y contraseña privada.");
            accounts.Add((email, password, role));
        }
        if (accounts.Select(a => a.Email).Distinct(StringComparer.OrdinalIgnoreCase).Count() != accounts.Count)
            throw new InvalidOperationException("Las cuentas iniciales deben tener correos diferentes.");

        var db = services.GetRequiredService<HelpdeskDbContext>();
        var users = services.GetRequiredService<UserManager<AppUser>>();
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        await using var transaction = await db.Database.BeginTransactionAsync();
        foreach (var role in new[] { Roles.Admin, Roles.Technician, Roles.Requester })
            if (!await roles.RoleExistsAsync(role)) Check(await roles.CreateAsync(new IdentityRole(role)));
        foreach (var account in accounts)
        {
            var existing = await users.FindByEmailAsync(account.Email);
            if (existing is not null)
            {
                if (!await users.IsInRoleAsync(existing, account.Role))
                    throw new InvalidOperationException("Una cuenta inicial existente tiene otro rol. No se modificaron sus permisos.");
                continue;
            }
            var user = new AppUser { UserName = account.Email, Email = account.Email, DisplayName = account.Role };
            Check(await users.CreateAsync(user, account.Password));
            Check(await users.AddToRoleAsync(user, account.Role));
        }
        await transaction.CommitAsync();
    }

    private static void Check(IdentityResult result)
    {
        if (!result.Succeeded)
            throw new InvalidOperationException("No se pudo crear la cuenta inicial: " + string.Join(", ", result.Errors.Select(e => e.Code)));
    }
}
