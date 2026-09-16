using System.Collections.Generic;
using BibliotecaApiMvcEf6.Models;

namespace BibliotecaApiMvcEf6.Repositories
{
    public interface ILibroRepository
    {
        List<Libro> ObtenerTodos();
        Libro ObtenerPorId(int id);
        void Agregar(Libro libro);
        void Actualizar(Libro libro);
    }
}
