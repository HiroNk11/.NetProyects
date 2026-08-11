using System.Collections.Generic;
using ProductosWinFormsEfReady.Models;
using ProductosWinFormsEfReady.Repositories;

namespace ProductosWinFormsEfReady.Services
{
    internal class ProductoService
    {
        private readonly IProductoRepository repository;

        public ProductoService(IProductoRepository repository)
        {
            this.repository = repository;
        }

        public List<Producto> ObtenerTodos()
        {
            return repository.ObtenerTodos();
        }

        public string Guardar(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (producto.Precio <= 0)
            {
                return "El precio debe ser mayor a cero.";
            }

            repository.Guardar(producto);
            return string.Empty;
        }

        public void Eliminar(int id)
        {
            repository.Eliminar(id);
        }
    }
}
