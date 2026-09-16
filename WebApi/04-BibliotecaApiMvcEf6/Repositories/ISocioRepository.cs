using System.Collections.Generic;
using BibliotecaApiMvcEf6.Models;

namespace BibliotecaApiMvcEf6.Repositories
{
    public interface ISocioRepository
    {
        List<Socio> ObtenerTodos();
        Socio ObtenerPorId(int id);
        void Agregar(Socio socio);
    }
}
