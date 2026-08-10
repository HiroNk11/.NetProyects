using System;
using System.Collections.Generic;
using System.Linq;

namespace RegistroVentas
{
    internal class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public List<ProductoVenta> Productos { get; } = new List<ProductoVenta>();

        public decimal CalcularTotal()
        {
            return Productos.Sum(producto => producto.CalcularSubtotal());
        }

        public string ObtenerResumen()
        {
            return $"{Id}. {Fecha:dd/MM/yyyy HH:mm} | Cliente: {Cliente} | Items: {Productos.Count} | Total: ${CalcularTotal():0.00}";
        }
    }
}
