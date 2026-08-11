using System.Collections.Generic;
using System.Linq;
using TurnosApiMvcEf6.Data;
using TurnosApiMvcEf6.Models;

namespace TurnosApiMvcEf6.Repositories
{
    public class EfProfesionalRepository : IProfesionalRepository
    {
        private readonly AppDbContext context;

        public EfProfesionalRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Profesional> ObtenerTodos()
        {
            return context.Profesionales.OrderBy(profesional => profesional.Nombre).ToList();
        }

        public Profesional ObtenerPorId(int id)
        {
            return context.Profesionales.Find(id);
        }

        public void Agregar(Profesional profesional)
        {
            context.Profesionales.Add(profesional);
            context.SaveChanges();
        }
    }
}
