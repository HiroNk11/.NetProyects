using System.Collections.Generic;
using System.Linq;
using TurnosApiMvcEf6.Data;
using TurnosApiMvcEf6.Models;

namespace TurnosApiMvcEf6.Repositories
{
    public class EfPacienteRepository : IPacienteRepository
    {
        private readonly AppDbContext context;

        public EfPacienteRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Paciente> ObtenerTodos()
        {
            return context.Pacientes.OrderBy(paciente => paciente.Nombre).ToList();
        }

        public Paciente ObtenerPorId(int id)
        {
            return context.Pacientes.Find(id);
        }

        public void Agregar(Paciente paciente)
        {
            context.Pacientes.Add(paciente);
            context.SaveChanges();
        }
    }
}
