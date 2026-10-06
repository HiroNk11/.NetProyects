using System.ComponentModel.DataAnnotations;
using Reservas.Api.Data;

namespace Reservas.Api;

public record CreateBooking(
    [Range(1, int.MaxValue)] int RoomId,
    [Required, StringLength(120, MinimumLength = 3)] string Title,
    [Range(1, 40)] int Attendees,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt);

public record BookingView(Guid Id, int RoomId, string Title, int Attendees,
    DateTimeOffset StartsAt, DateTimeOffset EndsAt, bool Cancelled)
{
    public static BookingView From(Booking b) => new(b.Id, b.RoomId, b.Title, b.Attendees,
        new DateTimeOffset(b.StartTicks, TimeSpan.Zero), new DateTimeOffset(b.EndTicks, TimeSpan.Zero), b.Cancelled);
}

public record BookingPage(int Page, int PageSize, int Total, IReadOnlyList<BookingView> Items);
public record BookingCreated(BookingView Booking, bool IsNew);
public class BookingProblem(int status, string message) : Exception(message)
{
    public int Status { get; } = status;
}
