using System.Collections.Generic;
using System.Linq;
using SistemaVentasWinFormsMvc.Models;

namespace SistemaVentasWinFormsMvc.Repositories
{
    internal class ProductoRepository : IProductoRepository
    {
        private readonly MemoryStore store;

        public ProductoRepository(MemoryStore store)
        {
            this.store = store;
        }

        public List<Producto> ObtenerTodos()
        {
            return store.Productos.ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            return store.Productos.FirstOrDefault(producto => producto.Id == id);
        }

        public void Agregar(Producto producto)
        {
            producto.Id = store.ProximoProductoId;
            store.ProximoProductoId++;
            store.Productos.Add(producto);
        }

        public void Actualizar(Producto producto)
        {
            Producto existente = ObtenerPorId(producto.Id);

            if (existente == null)
            {
                return;
            }

            existente.Nombre = producto.Nombre;
            existente.Precio = producto.Precio;
            existente.Stock = producto.Stock;
        }
    }
}
