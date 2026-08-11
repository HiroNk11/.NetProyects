using System.Collections.Generic;
using PedidosApiMvcEf6.Models;

namespace PedidosApiMvcEf6.Repositories
{
    public interface IPedidoRepository
    {
        List<Pedido> ObtenerTodos();
        Pedido ObtenerPorId(int id);
        void Agregar(Pedido pedido);
        void Actualizar(Pedido pedido);
    }
}
