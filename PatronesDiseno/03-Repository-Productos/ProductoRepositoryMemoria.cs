using System.Collections.Generic;
using System.Linq;

namespace RepositoryProductos
{
    internal class ProductoRepositoryMemoria : IProductoRepository
    {
        private readonly List<Producto> _productos = new List<Producto>();

        public List<Producto> ObtenerTodos()
        {
            return _productos.ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            return _productos.FirstOrDefault(p => p.Id == id);
        }

        public void Agregar(Producto producto)
        {
            _productos.Add(producto);
        }

        public void Actualizar(Producto producto)
        {
            Producto existente = ObtenerPorId(producto.Id);

            if (existente == null)
            {
                _productos.Add(producto);
            }
        }
    }
}
