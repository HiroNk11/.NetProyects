using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Reservas.Api;
using Reservas.Api.Data;
using Reservas.Api.Security;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ApiAccess>().BindConfiguration("ApiAccess")
    .Validate(o => o.Key.Length is >= 32 and <= 512 && !string.IsNullOrWhiteSpace(o.Key),
        "Configurá ApiAccess__Key con una credencial privada de 32 a 512 caracteres.").ValidateOnStart();
builder.Services.AddAuthentication("Integration")
    .AddScheme<AuthenticationSchemeOptions, ApiAuthentication>("Integration", _ => { });
builder.Services.AddAuthorization();
builder.Services.AddDbContext<BookingDb>((services, options) =>
{
    var connection = services.GetRequiredService<IConfiguration>().GetConnectionString("Bookings");
    if (string.IsNullOrWhiteSpace(connection))
    {
        var path = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(path);
        connection = $"Data Source={Path.Combine(path, "reservas.db")}";
    }
    options.UseSqlite(connection);
});
builder.Services.AddScoped<BookingService>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiErrors>();
builder.Services.AddHealthChecks();
var app = builder.Build();
app.UseExceptionHandler();
if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/healthz").AllowAnonymous();
app.MapOpenApi().RequireAuthorization();
app.MapGet("/api/rooms", async (BookingDb db, CancellationToken ct) =>
    await db.Rooms.AsNoTracking().OrderBy(r => r.Id).ToListAsync(ct)).RequireAuthorization();
app.MapControllers();
if (!app.Environment.IsEnvironment("Testing"))
{
    await using var scope = app.Services.CreateAsyncScope();
    await scope.ServiceProvider.GetRequiredService<BookingDb>().Database.MigrateAsync();
}
app.Run();
public partial class Program { }
