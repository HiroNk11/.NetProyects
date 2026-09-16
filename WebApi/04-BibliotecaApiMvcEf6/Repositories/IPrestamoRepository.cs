using System.Collections.Generic;
using BibliotecaApiMvcEf6.Models;

namespace BibliotecaApiMvcEf6.Repositories
{
    public interface IPrestamoRepository
    {
        List<Prestamo> ObtenerTodos();
        Prestamo ObtenerPorId(int id);
        void Agregar(Prestamo prestamo);
        void Actualizar(Prestamo prestamo);
    }
}
