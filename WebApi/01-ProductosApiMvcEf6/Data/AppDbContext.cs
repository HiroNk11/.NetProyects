using System.Data.Entity;
using ProductosApiMvcEf6.Models;

namespace ProductosApiMvcEf6.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=ProductosDb")
        {
        }

        public DbSet<Producto> Productos { get; set; }
    }
}
