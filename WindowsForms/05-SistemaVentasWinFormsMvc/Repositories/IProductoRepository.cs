using System.Collections.Generic;
using SistemaVentasWinFormsMvc.Models;

namespace SistemaVentasWinFormsMvc.Repositories
{
    internal interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        Producto ObtenerPorId(int id);
        void Agregar(Producto producto);
        void Actualizar(Producto producto);
    }
}
