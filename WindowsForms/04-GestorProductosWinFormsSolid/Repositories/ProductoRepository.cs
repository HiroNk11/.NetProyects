using System.Collections.Generic;
using System.Linq;
using GestorProductosWinFormsSolid.Models;

namespace GestorProductosWinFormsSolid.Repositories
{
    internal class ProductoRepository : IProductoRepository
    {
        private readonly List<Producto> productos = new List<Producto>();
        private int proximoId = 1;

        public List<Producto> ObtenerTodos()
        {
            return productos.ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            return productos.FirstOrDefault(producto => producto.Id == id);
        }

        public void Guardar(Producto producto)
        {
            if (producto.Id == 0)
            {
                producto.Id = proximoId;
                proximoId++;
                productos.Add(producto);
                return;
            }

            Producto existente = ObtenerPorId(producto.Id);

            if (existente == null)
            {
                return;
            }

            existente.Nombre = producto.Nombre;
            existente.Categoria = producto.Categoria;
            existente.Precio = producto.Precio;
            existente.Stock = producto.Stock;
        }

        public void Eliminar(int id)
        {
            Producto producto = ObtenerPorId(id);

            if (producto != null)
            {
                productos.Remove(producto);
            }
        }
    }
}
