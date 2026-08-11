using System.Collections.Generic;
using SistemaVentasWinFormsMvc.Models;
using SistemaVentasWinFormsMvc.Services;

namespace SistemaVentasWinFormsMvc.Controllers
{
    internal class VentaController
    {
        private readonly VentaService service;

        public VentaController(VentaService service)
        {
            this.service = service;
        }

        public string AgregarCliente(string nombre, string email)
        {
            return service.AgregarCliente(nombre, email);
        }

        public string AgregarProducto(string nombre, decimal precio, int stock)
        {
            return service.AgregarProducto(nombre, precio, stock);
        }

        public string RegistrarVenta(Cliente cliente, List<ItemVenta> items)
        {
            return service.RegistrarVenta(cliente, items);
        }

        public List<Cliente> ObtenerClientes()
        {
            return service.ObtenerClientes();
        }

        public List<Producto> ObtenerProductos()
        {
            return service.ObtenerProductos();
        }

        public List<Venta> ObtenerVentas()
        {
            return service.ObtenerVentas();
        }

        public decimal CalcularTotalVendido()
        {
            return service.CalcularTotalVendido();
        }
    }
}
