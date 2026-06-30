using System.Text;

namespace GeneradorReportes
{
    internal class ReporteTextoFormatter : IReporteFormatter
    {
        public string Formatear(ReporteVentas reporte)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Reporte de ventas");
            sb.AppendLine("----------------");

            foreach (Venta venta in reporte.Ventas)
            {
                sb.AppendLine($"{venta.Producto} | Cantidad: {venta.Cantidad} | Total: ${venta.Total:0.00}");
            }

            sb.AppendLine("----------------");
            sb.AppendLine($"Total general: ${reporte.TotalVentas:0.00}");

            return sb.ToString();
        }
    }
}
