using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BibliotecaApiMvcEf6.Data;
using BibliotecaApiMvcEf6.Models;

namespace BibliotecaApiMvcEf6.Repositories
{
    public class EfLibroRepository : ILibroRepository
    {
        private readonly AppDbContext context;

        public EfLibroRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Libro> ObtenerTodos()
        {
            return context.Libros.OrderBy(libro => libro.Titulo).ToList();
        }

        public Libro ObtenerPorId(int id)
        {
            return context.Libros.Find(id);
        }

        public void Agregar(Libro libro)
        {
            context.Libros.Add(libro);
            context.SaveChanges();
        }

        public void Actualizar(Libro libro)
        {
            context.Entry(libro).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
