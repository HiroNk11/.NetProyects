using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MesaAyuda.Web.Data;

public class SqlServerHelpdeskDbContext(DbContextOptions<SqlServerHelpdeskDbContext> options)
    : HelpdeskDbContext(options);

public class SqlServerHelpdeskDesignFactory : IDesignTimeDbContextFactory<SqlServerHelpdeskDbContext>
{
    public SqlServerHelpdeskDbContext CreateDbContext(string[] args)
    {
        var connection = Environment.GetEnvironmentVariable("ConnectionStrings__Helpdesk")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=MesaAyuda;Integrated Security=true;Encrypt=true";
        return new(new DbContextOptionsBuilder<SqlServerHelpdeskDbContext>().UseSqlServer(connection).Options);
    }
}
