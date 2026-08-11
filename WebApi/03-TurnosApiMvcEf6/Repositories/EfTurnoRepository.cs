using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TurnosApiMvcEf6.Data;
using TurnosApiMvcEf6.Models;

namespace TurnosApiMvcEf6.Repositories
{
    public class EfTurnoRepository : ITurnoRepository
    {
        private readonly AppDbContext context;

        public EfTurnoRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Turno> ObtenerTodos()
        {
            return context.Turnos
                .Include(turno => turno.Paciente)
                .Include(turno => turno.Profesional)
                .OrderBy(turno => turno.FechaHora)
                .ToList();
        }

        public Turno ObtenerPorId(int id)
        {
            return context.Turnos
                .Include(turno => turno.Paciente)
                .Include(turno => turno.Profesional)
                .FirstOrDefault(turno => turno.Id == id);
        }

        public List<Turno> ObtenerPorProfesional(int profesionalId)
        {
            return context.Turnos
                .Where(turno => turno.ProfesionalId == profesionalId)
                .ToList();
        }

        public void Agregar(Turno turno)
        {
            context.Turnos.Add(turno);
            context.SaveChanges();
        }

        public void Actualizar(Turno turno)
        {
            context.Entry(turno).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
