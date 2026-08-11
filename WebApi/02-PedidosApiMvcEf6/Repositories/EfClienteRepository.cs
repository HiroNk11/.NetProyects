using System.Collections.Generic;
using System.Linq;
using PedidosApiMvcEf6.Data;
using PedidosApiMvcEf6.Models;

namespace PedidosApiMvcEf6.Repositories
{
    public class EfClienteRepository : IClienteRepository
    {
        private readonly AppDbContext context;

        public EfClienteRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Cliente> ObtenerTodos()
        {
            return context.Clientes.OrderBy(cliente => cliente.Nombre).ToList();
        }

        public Cliente ObtenerPorId(int id)
        {
            return context.Clientes.Find(id);
        }

        public void Agregar(Cliente cliente)
        {
            context.Clientes.Add(cliente);
            context.SaveChanges();
        }
    }
}
