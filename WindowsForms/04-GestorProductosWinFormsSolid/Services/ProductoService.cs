using System;
using System.Collections.Generic;
using System.Linq;
using GestorProductosWinFormsSolid.Models;
using GestorProductosWinFormsSolid.Repositories;

namespace GestorProductosWinFormsSolid.Services
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

        public List<Producto> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return ObtenerTodos();
            }

            return repository.ObtenerTodos()
                .Where(producto =>
                    producto.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    producto.Categoria.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public string Guardar(Producto producto)
        {
            string error = Validar(producto);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            repository.Guardar(producto);
            return string.Empty;
        }

        public void Eliminar(int id)
        {
            repository.Eliminar(id);
        }

        public decimal CalcularValorTotal()
        {
            return repository.ObtenerTodos().Sum(producto => producto.ValorStock);
        }

        private string Validar(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(producto.Categoria))
            {
                return "La categoria es obligatoria.";
            }

            if (producto.Precio < 0)
            {
                return "El precio no puede ser negativo.";
            }

            if (producto.Stock < 0)
            {
                return "El stock no puede ser negativo.";
            }

            return string.Empty;
        }
    }
}
