using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BibliotecaApiMvcEf6.Data;
using BibliotecaApiMvcEf6.Models;

namespace BibliotecaApiMvcEf6.Repositories
{
    public class EfPrestamoRepository : IPrestamoRepository
    {
        private readonly AppDbContext context;

        public EfPrestamoRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Prestamo> ObtenerTodos()
        {
            return context.Prestamos
                .Include(prestamo => prestamo.Socio)
                .Include(prestamo => prestamo.Libro)
                .OrderByDescending(prestamo => prestamo.FechaPrestamo)
                .ToList();
        }

        public Prestamo ObtenerPorId(int id)
        {
            return context.Prestamos
                .Include(prestamo => prestamo.Socio)
                .Include(prestamo => prestamo.Libro)
                .FirstOrDefault(prestamo => prestamo.Id == id);
        }

        public void Agregar(Prestamo prestamo)
        {
            context.Prestamos.Add(prestamo);
            context.SaveChanges();
        }

        public void Actualizar(Prestamo prestamo)
        {
            context.Entry(prestamo).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
