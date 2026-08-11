using System.Collections.Generic;
using GestorProductosWinFormsSolid.Models;

namespace GestorProductosWinFormsSolid.Repositories
{
    internal interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        Producto ObtenerPorId(int id);
        void Guardar(Producto producto);
        void Eliminar(int id);
    }
}
