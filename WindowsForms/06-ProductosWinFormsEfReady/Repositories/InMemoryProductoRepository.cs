using System.Collections.Generic;
using System.Linq;
using ProductosWinFormsEfReady.Models;

namespace ProductosWinFormsEfReady.Repositories
{
    internal class InMemoryProductoRepository : IProductoRepository
    {
        private readonly List<Producto> productos = new List<Producto>();
        private int proximoId = 1;

        public List<Producto> ObtenerTodos()
        {
            return productos.ToList();
        }

        public void Guardar(Producto producto)
        {
            Producto existente = productos.FirstOrDefault(item => item.Id == producto.Id);

            if (existente == null)
            {
                producto.Id = proximoId;
                proximoId++;
                productos.Add(producto);
                return;
            }

            existente.Nombre = producto.Nombre;
            existente.Precio = producto.Precio;
            existente.Activo = producto.Activo;
        }

        public void Eliminar(int id)
        {
            Producto producto = productos.FirstOrDefault(item => item.Id == id);

            if (producto != null)
            {
                productos.Remove(producto);
            }
        }
    }
}
