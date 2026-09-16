using System.Collections.Generic;
using System.Linq;
using InventarioCleanArchitecture.Application.Interfaces;
using InventarioCleanArchitecture.Domain;

namespace InventarioCleanArchitecture.Infrastructure
{
    internal class InMemoryProductoRepository : IProductoRepository
    {
        private readonly List<Producto> _productos = new List<Producto>();

        public void Agregar(Producto producto)
        {
            _productos.Add(producto);
        }

        public Producto BuscarPorId(int id)
        {
            return _productos.FirstOrDefault(producto => producto.Id == id);
        }

        public IReadOnlyList<Producto> Listar()
        {
            return _productos
                .OrderBy(producto => producto.Nombre)
                .ToList();
        }

        public bool Existe(int id)
        {
            return _productos.Any(producto => producto.Id == id);
        }
    }
}
