using System.Collections.Generic;

namespace RepositoryProductos
{
    internal interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        Producto ObtenerPorId(int id);
        void Agregar(Producto producto);
        void Actualizar(Producto producto);
    }
}
