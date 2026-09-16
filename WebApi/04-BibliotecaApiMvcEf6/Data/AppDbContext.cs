using System.Data.Entity;
using BibliotecaApiMvcEf6.Models;

namespace BibliotecaApiMvcEf6.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=BibliotecaDb")
        {
        }

        public DbSet<Socio> Socios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prestamo>()
                .HasRequired(prestamo => prestamo.Socio)
                .WithMany(socio => socio.Prestamos)
                .HasForeignKey(prestamo => prestamo.SocioId);

            modelBuilder.Entity<Prestamo>()
                .HasRequired(prestamo => prestamo.Libro)
                .WithMany(libro => libro.Prestamos)
                .HasForeignKey(prestamo => prestamo.LibroId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
