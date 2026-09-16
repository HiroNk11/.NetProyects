using System;

namespace ReportesVentasLinq
{
    internal class Venta
    {
        public Venta(DateTime fecha, string vendedor, string region, string producto, int cantidad, decimal precioUnitario)
        {
            Fecha = fecha;
            Vendedor = vendedor;
            Region = region;
            Producto = producto;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }

        public DateTime Fecha { get; }

        public string Vendedor { get; }

        public string Region { get; }

        public string Producto { get; }

        public int Cantidad { get; }

        public decimal PrecioUnitario { get; }

        public decimal Total => Cantidad * PrecioUnitario;
    }
}
