using MesaAyuda.Web.Data;
using MesaAyuda.Web.Models;
using MesaAyuda.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Configuration["Database:Provider"] ?? "SQLite";
if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<SqlServerHelpdeskDbContext>((services, options) =>
        options.UseSqlServer(services.GetRequiredService<IConfiguration>().GetConnectionString("Helpdesk")
            ?? throw new InvalidOperationException("Falta ConnectionStrings:Helpdesk para SqlServer.")));
    builder.Services.AddScoped<HelpdeskDbContext>(services => services.GetRequiredService<SqlServerHelpdeskDbContext>());
}
else if (provider.Equals("SQLite", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<HelpdeskDbContext>((services, options) =>
    {
        var connection = services.GetRequiredService<IConfiguration>().GetConnectionString("Helpdesk");
        if (string.IsNullOrWhiteSpace(connection))
        {
            var directory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
            Directory.CreateDirectory(directory);
            connection = $"Data Source={Path.Combine(directory, "mesa-ayuda.db")}";
        }
        options.UseSqlite(connection);
    });
}
else throw new InvalidOperationException("Database:Provider debe ser SQLite o SqlServer.");
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 12;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
}).AddEntityFrameworkStores<HelpdeskDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Denied";
    options.Cookie.Name = "MesaAyuda.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
});
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
builder.Services.AddScoped<TicketService>();
builder.Services.AddSingleton(TimeProvider.System);
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Tickets}/{action=Index}/{id?}");
if (!app.Environment.IsEnvironment("Testing"))
{
    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<HelpdeskDbContext>();
    await db.Database.MigrateAsync();
    if (app.Environment.IsDevelopment() && app.Configuration.GetValue<bool>("Demo:Enabled"))
        await DemoData.InitializeAsync(scope.ServiceProvider);
    else
        await BootstrapAccounts.InitializeAsync(scope.ServiceProvider, app.Configuration);
}
app.Run();
public partial class Program { }
