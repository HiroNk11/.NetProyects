using System;
using System.Collections.Generic;
using System.Linq;
using InventarioCleanArchitecture.Application.Interfaces;
using InventarioCleanArchitecture.Domain;

namespace InventarioCleanArchitecture.Application.Services
{
    internal class InventarioService
    {
        private readonly IProductoRepository _repository;

        public InventarioService(IProductoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void AgregarProducto(Producto producto)
        {
            if (_repository.Existe(producto.Id))
            {
                throw new InvalidOperationException("Ya existe un producto con ese id.");
            }

            _repository.Agregar(producto);
        }

        public IReadOnlyList<Producto> ListarProductos()
        {
            return _repository.Listar();
        }

        public bool RegistrarEntrada(int id, int cantidad)
        {
            Producto producto = _repository.BuscarPorId(id);

            if (producto == null)
            {
                return false;
            }

            producto.AgregarStock(cantidad);
            return true;
        }

        public bool RegistrarSalida(int id, int cantidad)
        {
            Producto producto = _repository.BuscarPorId(id);
            return producto != null && producto.DescontarStock(cantidad);
        }

        public IReadOnlyList<Producto> ObtenerProductosConStockBajo()
        {
            return _repository
                .Listar()
                .Where(producto => producto.TieneStockBajo())
                .ToList();
        }

        public decimal CalcularValorTotal()
        {
            return _repository
                .Listar()
                .Sum(producto => producto.CalcularValorStock());
        }
    }
}
