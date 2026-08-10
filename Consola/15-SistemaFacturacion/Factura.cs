using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaFacturacion
{
    internal class Factura
    {
        private const decimal PorcentajeIva = 0.21m;

        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public List<ItemFactura> Items { get; } = new List<ItemFactura>();

        public decimal CalcularSubtotal()
        {
            return Items.Sum(item => item.CalcularSubtotal());
        }

        public decimal CalcularIva()
        {
            return CalcularSubtotal() * PorcentajeIva;
        }

        public decimal CalcularTotal()
        {
            return CalcularSubtotal() + CalcularIva();
        }

        public string ObtenerResumen()
        {
            return $"Factura {Numero} | {Fecha:dd/MM/yyyy HH:mm} | {Cliente.RazonSocial} | Total: ${CalcularTotal():0.00}";
        }
    }
}
