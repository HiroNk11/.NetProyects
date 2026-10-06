using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Reservas.Api;
using Reservas.Api.Data;

namespace Reservas.Tests;

public class BookingTests : IAsyncLifetime
{
    private readonly BookingFactory factory = new();
    private readonly DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(2);
    public Task InitializeAsync() => factory.InitializeDatabaseAsync();
    public Task DisposeAsync() => factory.CleanupAsync();
    private CreateBooking Input() => new(1, "Planificación semanal", 4, start, start.AddHours(1));
    private static Task<HttpResponseMessage> Create(HttpClient client, CreateBooking input, string? key = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/bookings") { Content = JsonContent.Create(input) };
        request.Headers.Add("Idempotency-Key", key ?? Guid.NewGuid().ToString());
        return client.SendAsync(request);
    }

    [Theory]
    [InlineData("/api/rooms")]
    [InlineData("/api/bookings")]
    [InlineData("/openapi/v1.json")]
    public async Task Protected_endpoints_require_credentials(string path)
    {
        using var client = factory.Client(false);
        var response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        client.DefaultRequestHeaders.Authorization = new("Bearer", "incorrect");
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync(path)).StatusCode);
    }

    [Fact]
    public async Task Anonymous_mutations_are_rejected_and_health_is_public()
    {
        using var client = factory.Client(false);
        Assert.Equal(HttpStatusCode.Unauthorized, (await Create(client, Input())).StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.PostAsync($"/api/bookings/{Guid.NewGuid()}/cancel", null)).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/healthz")).StatusCode);
    }

    [Fact]
    public void Startup_rejects_missing_private_credential()
    {
        using var host = factory.WithWebHostBuilder(b => b.ConfigureAppConfiguration((_, c) =>
            c.AddInMemoryCollection(new Dictionary<string, string?> { ["ApiAccess:Key"] = "" })));
        Assert.ThrowsAny<Exception>(() => host.CreateClient());
    }

    [Fact]
    public async Task Create_returns_location_and_persists_in_a_new_request()
    {
        using var client = factory.Client();
        var rooms = await client.GetFromJsonAsync<List<Room>>("/api/rooms");
        Assert.Equal(3, rooms!.Count);
        var response = await Create(client, Input());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<BookingView>();
        using var another = factory.Client();
        Assert.Equal(created, await another.GetFromJsonAsync<BookingView>(response.Headers.Location));
    }

    [Fact]
    public async Task Openapi_describes_the_actual_routes()
    {
        using var client = factory.Client();
        var response = await client.GetAsync("/openapi/v1.json");
        response.EnsureSuccessStatusCode();
        var document = await response.Content.ReadAsStringAsync();
        Assert.Contains("/api/bookings/{id}/cancel", document);
        Assert.Contains("Idempotency-Key", document);
    }

    [Theory]
    [InlineData(-30, 30)]
    [InlineData(30, 90)]
    [InlineData(-30, 90)]
    [InlineData(10, 20)]
    public async Task Overlapping_intervals_are_rejected(int from, int until)
    {
        using var client = factory.Client();
        Assert.Equal(HttpStatusCode.Created, (await Create(client, Input())).StatusCode);
        var response = await Create(client, Input() with { StartsAt = start.AddMinutes(from), EndsAt = start.AddMinutes(until) });
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Adjacent_intervals_and_different_rooms_are_allowed()
    {
        using var client = factory.Client();
        Assert.Equal(HttpStatusCode.Created, (await Create(client, Input())).StatusCode);
        Assert.Equal(HttpStatusCode.Created, (await Create(client, Input() with { StartsAt = start.AddHours(1), EndsAt = start.AddHours(2) })).StatusCode);
        Assert.Equal(HttpStatusCode.Created, (await Create(client, Input() with { RoomId = 2 })).StatusCode);
    }

    [Fact]
    public async Task Idempotency_replays_original_and_rejects_changed_payload()
    {
        using var client = factory.Client();
        var key = Guid.NewGuid().ToString();
        var original = await (await Create(client, Input(), key)).Content.ReadFromJsonAsync<BookingView>();
        var replay = await Create(client, Input(), key);
        Assert.Equal(HttpStatusCode.OK, replay.StatusCode);
        Assert.Equal(original, await replay.Content.ReadFromJsonAsync<BookingView>());
        Assert.Equal(HttpStatusCode.Conflict, (await Create(client, Input() with { Title = "Otra reunión" }, key)).StatusCode);
        Assert.Equal(1, (await client.GetFromJsonAsync<BookingPage>("/api/bookings"))!.Total);
    }

    [Fact]
    public async Task Cancellation_is_repeatable_releases_slot_and_replay_does_not_reactivate()
    {
        using var client = factory.Client();
        var key = Guid.NewGuid().ToString();
        var original = (await (await Create(client, Input(), key)).Content.ReadFromJsonAsync<BookingView>())!;
        for (var i = 0; i < 2; i++)
            Assert.Equal(HttpStatusCode.NoContent, (await client.PostAsync($"/api/bookings/{original.Id}/cancel", null)).StatusCode);
        var replay = await Create(client, Input(), key);
        Assert.Equal(HttpStatusCode.OK, replay.StatusCode);
        Assert.True((await replay.Content.ReadFromJsonAsync<BookingView>())!.Cancelled);
        Assert.Equal(HttpStatusCode.Created, (await Create(client, Input())).StatusCode);
    }

    [Fact]
    public async Task Parallel_conflicting_requests_create_only_one_booking()
    {
        using var first = factory.Client();
        using var second = factory.Client();
        var responses = await Task.WhenAll(Task.Run(() => Create(first, Input())), Task.Run(() => Create(second, Input())));
        Assert.Single(responses, r => r.StatusCode == HttpStatusCode.Created);
        Assert.Single(responses, r => r.StatusCode == HttpStatusCode.Conflict);
        Assert.Equal(1, (await first.GetFromJsonAsync<BookingPage>("/api/bookings"))!.Total);
    }

    [Fact]
    public async Task Parallel_retries_with_same_key_return_same_booking()
    {
        using var first = factory.Client();
        using var second = factory.Client();
        var key = Guid.NewGuid().ToString();
        var responses = await Task.WhenAll(Task.Run(() => Create(first, Input(), key)), Task.Run(() => Create(second, Input(), key)));
        Assert.Single(responses, r => r.StatusCode == HttpStatusCode.Created);
        Assert.Single(responses, r => r.StatusCode == HttpStatusCode.OK);
        var firstView = await responses[0].Content.ReadFromJsonAsync<BookingView>();
        Assert.Equal(firstView, await responses[1].Content.ReadFromJsonAsync<BookingView>());
    }

    [Theory]
    [InlineData("title")]
    [InlineData("capacity")]
    [InlineData("duration")]
    [InlineData("past")]
    [InlineData("negative")]
    [InlineData("zero")]
    public async Task Invalid_reservations_are_not_saved(string invalid)
    {
        var input = invalid switch
        {
            "title" => Input() with { Title = "   " },
            "capacity" => Input() with { Attendees = 7 },
            "duration" => Input() with { EndsAt = start.AddHours(25) },
            "past" => Input() with { StartsAt = DateTimeOffset.UtcNow.AddDays(-1) },
            "negative" => Input() with { Attendees = -1 },
            _ => Input() with { EndsAt = start }
        };
        using var client = factory.Client();
        Assert.Equal(HttpStatusCode.BadRequest, (await Create(client, input)).StatusCode);
        Assert.Equal(0, (await client.GetFromJsonAsync<BookingPage>("/api/bookings"))!.Total);
    }

    [Fact]
    public async Task Missing_keys_malformed_json_and_missing_resources_return_client_errors()
    {
        using var client = factory.Client();
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/api/bookings", Input())).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await Create(client, Input(), "not-a-uuid")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsync("/api/bookings", new StringContent("{", System.Text.Encoding.UTF8, "application/json"))).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await Create(client, Input() with { RoomId = 999 })).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/bookings/{Guid.NewGuid()}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.PostAsync($"/api/bookings/{Guid.NewGuid()}/cancel", null)).StatusCode);
    }

    [Fact]
    public async Task Pagination_filters_and_utc_normalization_work()
    {
        using var client = factory.Client();
        var offsetInput = Input() with { StartsAt = start.ToOffset(TimeSpan.FromHours(-3)), EndsAt = start.AddHours(1).ToOffset(TimeSpan.FromHours(-3)) };
        var first = (await (await Create(client, offsetInput)).Content.ReadFromJsonAsync<BookingView>())!;
        Assert.Equal(TimeSpan.Zero, first.StartsAt.Offset);
        Assert.Equal(start, first.StartsAt);
        await Create(client, Input() with { RoomId = 2 });
        await client.PostAsync($"/api/bookings/{first.Id}/cancel", null);
        var all = (await client.GetFromJsonAsync<BookingPage>("/api/bookings?pageSize=1&page=2"))!;
        Assert.Equal(2, all.Total);
        Assert.Single(all.Items);
        var filtered = (await client.GetFromJsonAsync<BookingPage>("/api/bookings?roomId=1&cancelled=true"))!;
        Assert.Equal(first.Id, Assert.Single(filtered.Items).Id);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.GetAsync("/api/bookings?pageSize=51")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.GetAsync("/api/bookings?page=0")).StatusCode);
    }
}
