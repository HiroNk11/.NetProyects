using System.Collections.Generic;
using ProductosWinFormsEfReady.Models;

namespace ProductosWinFormsEfReady.Repositories
{
    internal interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        void Guardar(Producto producto);
        void Eliminar(int id);
    }
}
