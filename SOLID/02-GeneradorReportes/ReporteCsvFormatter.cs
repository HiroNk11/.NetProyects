using System.Globalization;
using System.Text;

namespace GeneradorReportes
{
    internal class ReporteCsvFormatter : IReporteFormatter
    {
        public string Formatear(ReporteVentas reporte)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Producto,Cantidad,PrecioUnitario,Total");

            foreach (Venta venta in reporte.Ventas)
            {
                sb.AppendLine($"{venta.Producto},{venta.Cantidad},{Formatear(venta.PrecioUnitario)},{Formatear(venta.Total)}");
            }

            return sb.ToString();
        }

        private static string Formatear(decimal valor)
        {
            return valor.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}
