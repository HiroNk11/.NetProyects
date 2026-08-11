using System.Collections.Generic;
using TurnosApiMvcEf6.Models;

namespace TurnosApiMvcEf6.Repositories
{
    public interface IPacienteRepository
    {
        List<Paciente> ObtenerTodos();
        Paciente ObtenerPorId(int id);
        void Agregar(Paciente paciente);
    }
}
