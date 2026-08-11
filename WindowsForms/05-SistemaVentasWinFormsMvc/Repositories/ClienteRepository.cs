using System.Collections.Generic;
using System.Linq;
using SistemaVentasWinFormsMvc.Models;

namespace SistemaVentasWinFormsMvc.Repositories
{
    internal class ClienteRepository : IClienteRepository
    {
        private readonly MemoryStore store;

        public ClienteRepository(MemoryStore store)
        {
            this.store = store;
        }

        public List<Cliente> ObtenerTodos()
        {
            return store.Clientes.ToList();
        }

        public void Agregar(Cliente cliente)
        {
            cliente.Id = store.ProximoClienteId;
            store.ProximoClienteId++;
            store.Clientes.Add(cliente);
        }
    }
}
