using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaVentasWinFormsMvc.Models
{
    internal class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public List<ItemVenta> Items { get; } = new List<ItemVenta>();
        public decimal Total => Items.Sum(item => item.Subtotal);
        public string ClienteNombre => Cliente.Nombre;
    }
}
