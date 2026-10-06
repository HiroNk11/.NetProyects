using Microsoft.EntityFrameworkCore;

namespace Reservas.Api.Data;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Capacity { get; set; }
}

public class Booking
{
    public Guid Id { get; set; }
    public int RoomId { get; set; }
    public string Title { get; set; } = "";
    public int Attendees { get; set; }
    public long StartTicks { get; set; }
    public long EndTicks { get; set; }
    public bool Cancelled { get; set; }
    public string RequestKey { get; set; } = "";
}

public class BookingDb(DbContextOptions<BookingDb> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Room>().Property(r => r.Name).HasMaxLength(80);
        model.Entity<Room>().HasData(new Room { Id = 1, Name = "Sala Norte", Capacity = 6 },
            new Room { Id = 2, Name = "Sala Sur", Capacity = 12 },
            new Room { Id = 3, Name = "Auditorio", Capacity = 40 });
        model.Entity<Booking>().Property(b => b.Title).HasMaxLength(120);
        model.Entity<Booking>().Property(b => b.RequestKey).HasMaxLength(32);
        model.Entity<Booking>().HasIndex(b => b.RequestKey).IsUnique();
        model.Entity<Booking>().HasIndex(b => new { b.RoomId, b.Cancelled, b.StartTicks, b.EndTicks });
        model.Entity<Booking>().HasOne<Room>().WithMany().HasForeignKey(b => b.RoomId).OnDelete(DeleteBehavior.Restrict);
    }
}
