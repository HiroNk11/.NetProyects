using System;
using System.Collections.Generic;
using System.Linq;
using SistemaVentasWinFormsMvc.Models;
using SistemaVentasWinFormsMvc.Repositories;

namespace SistemaVentasWinFormsMvc.Services
{
    internal class VentaService
    {
        private readonly IClienteRepository clienteRepository;
        private readonly IProductoRepository productoRepository;
        private readonly IVentaRepository ventaRepository;

        public VentaService(IClienteRepository clienteRepository, IProductoRepository productoRepository, IVentaRepository ventaRepository)
        {
            this.clienteRepository = clienteRepository;
            this.productoRepository = productoRepository;
            this.ventaRepository = ventaRepository;
        }

        public string AgregarCliente(string nombre, string email)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre del cliente es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                return "Ingrese un email valido.";
            }

            clienteRepository.Agregar(new Cliente { Nombre = nombre.Trim(), Email = email.Trim() });
            return string.Empty;
        }

        public string AgregarProducto(string nombre, decimal precio, int stock)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return "El nombre del producto es obligatorio.";
            }

            if (precio <= 0)
            {
                return "El precio debe ser mayor a cero.";
            }

            if (stock < 0)
            {
                return "El stock no puede ser negativo.";
            }

            productoRepository.Agregar(new Producto { Nombre = nombre.Trim(), Precio = precio, Stock = stock });
            return string.Empty;
        }

        public string RegistrarVenta(Cliente cliente, List<ItemVenta> items)
        {
            if (cliente == null)
            {
                return "Seleccione un cliente.";
            }

            if (items.Count == 0)
            {
                return "Agregue al menos un item.";
            }

            foreach (ItemVenta item in items)
            {
                Producto producto = productoRepository.ObtenerPorId(item.Producto.Id);

                if (producto == null || item.Cantidad > producto.Stock)
                {
                    return $"Stock insuficiente para {item.Producto.Nombre}.";
                }
            }

            foreach (ItemVenta item in items)
            {
                Producto producto = productoRepository.ObtenerPorId(item.Producto.Id);
                producto.Stock -= item.Cantidad;
                productoRepository.Actualizar(producto);
            }

            Venta venta = new Venta { Cliente = cliente, Fecha = DateTime.Now };
            venta.Items.AddRange(items);
            ventaRepository.Agregar(venta);
            return string.Empty;
        }

        public List<Cliente> ObtenerClientes()
        {
            return clienteRepository.ObtenerTodos();
        }

        public List<Producto> ObtenerProductos()
        {
            return productoRepository.ObtenerTodos();
        }

        public List<Venta> ObtenerVentas()
        {
            return ventaRepository.ObtenerTodas();
        }

        public decimal CalcularTotalVendido()
        {
            return ventaRepository.ObtenerTodas().Sum(venta => venta.Total);
        }
    }
}
