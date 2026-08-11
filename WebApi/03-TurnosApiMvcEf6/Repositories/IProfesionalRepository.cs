using System.Collections.Generic;
using TurnosApiMvcEf6.Models;

namespace TurnosApiMvcEf6.Repositories
{
    public interface IProfesionalRepository
    {
        List<Profesional> ObtenerTodos();
        Profesional ObtenerPorId(int id);
        void Agregar(Profesional profesional);
    }
}
