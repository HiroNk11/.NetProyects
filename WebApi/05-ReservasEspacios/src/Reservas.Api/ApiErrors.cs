using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Reservas.Api;

public class ApiErrors : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is BookingProblem problem)
        {
            await Results.Problem(statusCode: problem.Status, title: problem.Message).ExecuteAsync(context);
            return true;
        }
        if (exception is SqliteException { SqliteErrorCode: 5 or 6 })
        {
            context.Response.Headers.RetryAfter = "1";
            await Results.Problem(statusCode: 503, title: "La base está ocupada. Reintentá con la misma clave de idempotencia.").ExecuteAsync(context);
            return true;
        }
        return false;
    }
}
