using MesaAyuda.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace MesaAyuda.Web.Data;
public static class DemoData
{
    public const string Password = "Demo.Soporte2026!";
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var users = services.GetRequiredService<UserManager<AppUser>>();
        var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
        var db = services.GetRequiredService<HelpdeskDbContext>();
        foreach (var role in new[] { Roles.Admin, Roles.Technician, Roles.Requester })
            if (!await roles.RoleExistsAsync(role)) Check(await roles.CreateAsync(new IdentityRole(role)));
        async Task<AppUser> Create(string email, string name, string role)
        {
            var user = await users.FindByEmailAsync(email);
            if (user is not null) return user;
            user = new AppUser { UserName = email, Email = email, DisplayName = name, EmailConfirmed = true };
            Check(await users.CreateAsync(user, Password));
            Check(await users.AddToRoleAsync(user, role));
            return user;
        }
        var admin = await Create("admin@mesa.local", "Alex · Administración", Roles.Admin);
        var technician = await Create("tecnico@mesa.local", "Valentina · Soporte", Roles.Technician);
        var requester = await Create("solicitante@mesa.local", "Nicolás · Operaciones", Roles.Requester);
        if (await db.Tickets.AnyAsync()) return;
        var samples = new[]
        {
            ("Sin acceso al sistema de ventas", "Al iniciar sesión en ventas aparece un mensaje de acceso denegado.", TicketPriority.Urgente, TicketStatus.Nuevo),
            ("Configurar correo en nueva notebook", "Necesito configurar la cuenta de trabajo en el equipo de reemplazo.", TicketPriority.Media, TicketStatus.EnCurso),
            ("Impresora de depósito sin conexión", "La impresora no aparece en la red desde esta mañana.", TicketPriority.Alta, TicketStatus.EnCurso),
            ("Actualizar permisos de reportes", "El reporte mensual ya está disponible para el equipo de operaciones.", TicketPriority.Baja, TicketStatus.Resuelto)
        };
        var now = DateTime.UtcNow;
        foreach (var (title, description, priority, status) in samples)
        {
            var ticket = new Ticket
            {
                Title = title, Description = description, Priority = priority, Status = status,
                RequesterId = requester.Id, TechnicianId = status == TicketStatus.Nuevo ? null : technician.Id,
                CreatedAt = now, UpdatedAt = now
            };
            ticket.Events.Add(new TicketEvent { ActorId = admin.Id, Description = "Escenario de demostración creado", CreatedAt = now });
            db.Tickets.Add(ticket);
        }
        await db.SaveChangesAsync();
    }
    private static void Check(IdentityResult result)
    {
        if (!result.Succeeded) throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
