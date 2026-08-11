using System.Collections.Generic;
using TurnosApiMvcEf6.Models;

namespace TurnosApiMvcEf6.Repositories
{
    public interface ITurnoRepository
    {
        List<Turno> ObtenerTodos();
        Turno ObtenerPorId(int id);
        List<Turno> ObtenerPorProfesional(int profesionalId);
        void Agregar(Turno turno);
        void Actualizar(Turno turno);
    }
}
