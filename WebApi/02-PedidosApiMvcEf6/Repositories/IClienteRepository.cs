using System.Collections.Generic;
using PedidosApiMvcEf6.Models;

namespace PedidosApiMvcEf6.Repositories
{
    public interface IClienteRepository
    {
        List<Cliente> ObtenerTodos();
        Cliente ObtenerPorId(int id);
        void Agregar(Cliente cliente);
    }
}
