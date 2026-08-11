using System.Collections.Generic;
using ProductosApiMvcEf6.Dtos;
using ProductosApiMvcEf6.Models;
using ProductosApiMvcEf6.Repositories;

namespace ProductosApiMvcEf6.Services
{
    public class ProductoService
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

        public Producto ObtenerPorId(int id)
        {
            return repository.ObtenerPorId(id);
        }

        public string Crear(ProductoCreateDto dto, out Producto producto)
        {
            producto = null;
            string error = Validar(dto.Nombre, dto.Precio, dto.Stock);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            producto = new Producto
            {
                Nombre = dto.Nombre.Trim(),
                Precio = dto.Precio,
                Stock = dto.Stock,
                Activo = true
            };

            repository.Agregar(producto);
            return string.Empty;
        }

        public string Actualizar(int id, ProductoUpdateDto dto, out Producto producto)
        {
            producto = repository.ObtenerPorId(id);

            if (producto == null)
            {
                return "Producto no encontrado.";
            }

            string error = Validar(dto.Nombre, dto.Precio, dto.Stock);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            producto.Nombre = dto.Nombre.Trim();
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
            producto.Activo = dto.Activo;

            repository.Actualizar(producto);
            return string.Empty;
        }

        public bool Eliminar(int id)
        {
            Producto producto = repository.ObtenerPorId(id);

            if (producto == null)
            {
                return false;
            }

            repository.Eliminar(producto);
            return true;
        }

        private string Validar(string nombre, decimal precio, int stock)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (precio <= 0)
            {
                return "El precio debe ser mayor a cero.";
            }

            if (stock < 0)
            {
                return "El stock no puede ser negativo.";
            }

            return string.Empty;
        }
    }
}
