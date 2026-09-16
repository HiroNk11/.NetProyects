using AdapterExportacion.Domain;

namespace AdapterExportacion.Exportadores
{
    internal class CsvExportador : IExportadorReporte
    {
        public string Exportar(ReporteVenta reporte)
        {
            return "Periodo,CantidadVentas,TotalFacturado" + "\n" +
                   $"{reporte.Periodo},{reporte.CantidadVentas},{reporte.TotalFacturado:0.00}";
        }
    }
}
