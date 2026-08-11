using System.Collections.Generic;
using PedidosApiMvcEf6.Dtos;
using PedidosApiMvcEf6.Models;
using PedidosApiMvcEf6.Repositories;

namespace PedidosApiMvcEf6.Services
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
