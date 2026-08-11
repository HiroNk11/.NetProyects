using System.Collections.Generic;
using ProductosApiMvcEf6.Models;

namespace ProductosApiMvcEf6.Repositories
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        Producto ObtenerPorId(int id);
        void Agregar(Producto producto);
        void Actualizar(Producto producto);
        void Eliminar(Producto producto);
    }
}
