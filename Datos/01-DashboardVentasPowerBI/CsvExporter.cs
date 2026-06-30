using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace DashboardVentasPowerBI
{
    internal static class CsvExporter
    {
        public static void ExportarClientes(string ruta, List<Cliente> clientes)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ClienteId,Nombre,Segmento,Provincia");

            foreach (Cliente cliente in clientes)
            {
                sb.AppendLine($"{cliente.Id},{cliente.Nombre},{cliente.Segmento},{cliente.Provincia}");
            }

            File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
        }

        public static void ExportarProductos(string ruta, List<Producto> productos)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ProductoId,Nombre,Categoria,Costo,Precio");

            foreach (Producto producto in productos)
            {
                sb.AppendLine($"{producto.Id},{producto.Nombre},{producto.Categoria},{Formatear(producto.Costo)},{Formatear(producto.Precio)}");
            }

            File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
        }

        public static void ExportarVentas(string ruta, List<Venta> ventas)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("VentaId,Fecha,ClienteId,Canal,Vendedor");

            foreach (Venta venta in ventas)
            {
                sb.AppendLine($"{venta.Id},{venta.Fecha:yyyy-MM-dd},{venta.ClienteId},{venta.Canal},{venta.Vendedor}");
            }

            File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
        }

        public static void ExportarDetalleVentas(string ruta, List<DetalleVenta> detalles)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("VentaId,ProductoId,Cantidad,PrecioUnitario,CostoUnitario");

            foreach (DetalleVenta detalle in detalles)
            {
                sb.AppendLine($"{detalle.VentaId},{detalle.ProductoId},{detalle.Cantidad},{Formatear(detalle.PrecioUnitario)},{Formatear(detalle.CostoUnitario)}");
            }

            File.WriteAllText(ruta, sb.ToString(), Encoding.UTF8);
        }

        private static string Formatear(decimal valor)
        {
            return valor.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}
