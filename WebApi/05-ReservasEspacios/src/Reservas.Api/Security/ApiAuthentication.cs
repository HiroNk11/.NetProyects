using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Reservas.Api.Security;

public class ApiAccess
{
    public string Key { get; set; } = "";
}

public class ApiAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
    UrlEncoder encoder, IOptions<ApiAccess> access) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var header = Request.Headers.Authorization;
        if (header.Count != 1 || !header.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.NoResult());
        var candidate = header.ToString()[7..];
        if (candidate.Length > 512 || !CryptographicOperations.FixedTimeEquals(
                SHA256.HashData(Encoding.UTF8.GetBytes(candidate)), SHA256.HashData(Encoding.UTF8.GetBytes(access.Value.Key))))
            return Task.FromResult(AuthenticateResult.Fail("Credencial inválida."));
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "integration")], Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)));
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.WWWAuthenticate = "Bearer";
        await Results.Problem(statusCode: 401, title: "Se requiere una credencial válida.").ExecuteAsync(Context);
    }
}
