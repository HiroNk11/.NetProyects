using System.Collections.Generic;
using PedidosApiMvcEf6.Models;

namespace PedidosApiMvcEf6.Repositories
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        Producto ObtenerPorId(int id);
        void Agregar(Producto producto);
        void Actualizar(Producto producto);
    }
}
