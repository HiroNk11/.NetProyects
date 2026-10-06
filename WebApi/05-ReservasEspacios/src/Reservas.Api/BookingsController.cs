using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Api.Data;

namespace Reservas.Api;

[ApiController, Authorize, Route("api/bookings")]
public class BookingsController(BookingDb db, BookingService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<BookingView>(201)]
    [ProducesResponseType<BookingView>(200)]
    [ProducesResponseType<ProblemDetails>(409)]
    public async Task<ActionResult<BookingView>> Create(CreateBooking input,
        [FromHeader(Name = "Idempotency-Key")] string? key, CancellationToken ct)
    {
        if (!Guid.TryParse(key, out var requestKey) || requestKey == Guid.Empty)
            return Problem(statusCode: 400, title: "Idempotency-Key debe ser un UUID no vacío.");
        var result = await service.CreateAsync(input, requestKey, ct);
        return result.IsNew ? CreatedAtAction(nameof(Get), new { id = result.Booking.Id }, result.Booking) : Ok(result.Booking);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookingView>> Get(Guid id, CancellationToken ct)
    {
        var booking = await db.Bookings.AsNoTracking().SingleOrDefaultAsync(b => b.Id == id, ct);
        return booking is null ? Problem(statusCode: 404, title: "La reserva no existe.") : Ok(BookingView.From(booking));
    }

    [HttpGet]
    public async Task<ActionResult<BookingPage>> List(CancellationToken ct, [FromQuery, Range(1, int.MaxValue)] int? roomId = null,
        [FromQuery] bool? cancelled = null, [FromQuery, Range(1, 1000000)] int page = 1,
        [FromQuery, Range(1, 50)] int pageSize = 20)
    {
        var query = db.Bookings.AsNoTracking();
        if (roomId.HasValue) query = query.Where(b => b.RoomId == roomId.Value);
        if (cancelled.HasValue) query = query.Where(b => b.Cancelled == cancelled.Value);
        var total = await query.CountAsync(ct);
        var items = await query.OrderBy(b => b.StartTicks).ThenBy(b => b.Id)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return Ok(new BookingPage(page, pageSize, total, items.Select(BookingView.From).ToList()));
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        await service.CancelAsync(id, ct);
        return NoContent();
    }
}
