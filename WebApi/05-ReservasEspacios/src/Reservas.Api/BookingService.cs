using Microsoft.EntityFrameworkCore;
using Reservas.Api.Data;

namespace Reservas.Api;

public class BookingService(BookingDb db, TimeProvider clock)
{
    public async Task<BookingCreated> CreateAsync(CreateBooking input, Guid requestKey, CancellationToken ct)
    {
        var title = input.Title.Trim();
        if (title.Length < 3) throw new BookingProblem(400, "El título debe tener al menos tres caracteres.");
        // SQLite adquiere el bloqueo de escritura antes de comprobar la disponibilidad.
        // La comprobación y la inserción deben permanecer dentro de esta transacción.
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        var key = requestKey.ToString("N");
        var existing = await db.Bookings.SingleOrDefaultAsync(b => b.RequestKey == key, ct);
        if (existing is not null)
        {
            if (existing.RoomId != input.RoomId || existing.Title != title || existing.Attendees != input.Attendees
                || existing.StartTicks != input.StartsAt.UtcTicks || existing.EndTicks != input.EndsAt.UtcTicks)
                throw new BookingProblem(409, "La clave de idempotencia ya se usó con otros datos.");
            return new(BookingView.From(existing), false);
        }
        if (input.StartsAt <= clock.GetUtcNow()) throw new BookingProblem(400, "La reserva debe comenzar en el futuro.");
        if (input.EndsAt <= input.StartsAt || input.EndsAt - input.StartsAt > TimeSpan.FromHours(24))
            throw new BookingProblem(400, "La duración debe ser positiva y no superar 24 horas.");
        var room = await db.Rooms.FindAsync([input.RoomId], ct)
            ?? throw new BookingProblem(404, "La sala no existe.");
        if (input.Attendees > room.Capacity) throw new BookingProblem(400, "La cantidad de asistentes supera la capacidad de la sala.");
        var start = input.StartsAt.UtcTicks;
        var end = input.EndsAt.UtcTicks;
        if (await db.Bookings.AnyAsync(b => b.RoomId == room.Id && !b.Cancelled && b.StartTicks < end && b.EndTicks > start, ct))
            throw new BookingProblem(409, "La sala ya tiene una reserva en ese horario.");
        var booking = new Booking { Id = Guid.NewGuid(), RoomId = room.Id, Title = title,
            Attendees = input.Attendees, StartTicks = start, EndTicks = end, RequestKey = key };
        db.Bookings.Add(booking);
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
        return new(BookingView.From(booking), true);
    }

    public async Task CancelAsync(Guid id, CancellationToken ct)
    {
        var affected = await db.Bookings.Where(b => b.Id == id)
            .ExecuteUpdateAsync(update => update.SetProperty(b => b.Cancelled, true), ct);
        if (affected == 0) throw new BookingProblem(404, "La reserva no existe.");
    }
}
