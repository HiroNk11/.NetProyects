using System.Collections.Generic;
using System.Linq;
using BibliotecaApiMvcEf6.Data;
using BibliotecaApiMvcEf6.Models;

namespace BibliotecaApiMvcEf6.Repositories
{
    public class EfSocioRepository : ISocioRepository
    {
        private readonly AppDbContext context;

        public EfSocioRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Socio> ObtenerTodos()
        {
            return context.Socios.OrderBy(socio => socio.Nombre).ToList();
        }

        public Socio ObtenerPorId(int id)
        {
            return context.Socios.Find(id);
        }

        public void Agregar(Socio socio)
        {
            context.Socios.Add(socio);
            context.SaveChanges();
        }
    }
}
