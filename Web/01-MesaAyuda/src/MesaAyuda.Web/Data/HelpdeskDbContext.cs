using MesaAyuda.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MesaAyuda.Web.Data;
public class HelpdeskDbContext(DbContextOptions<HelpdeskDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> Comments => Set<TicketComment>();
    public DbSet<TicketEvent> Events => Set<TicketEvent>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Ticket>().HasOne(t => t.Requester).WithMany()
            .HasForeignKey(t => t.RequesterId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Ticket>().HasOne(t => t.Technician).WithMany()
            .HasForeignKey(t => t.TechnicianId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<TicketComment>().HasOne(c => c.Author).WithMany()
            .HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<TicketEvent>().HasOne(e => e.Actor).WithMany()
            .HasForeignKey(e => e.ActorId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Ticket>().HasIndex(t => new { t.RequesterId, t.Status });
        builder.Entity<Ticket>().HasIndex(t => new { t.TechnicianId, t.Status });
        builder.Entity<Ticket>().HasIndex(t => t.UpdatedAt);
    }
}
