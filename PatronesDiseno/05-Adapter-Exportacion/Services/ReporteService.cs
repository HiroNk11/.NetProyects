using AdapterExportacion.Domain;
using AdapterExportacion.Exportadores;

namespace AdapterExportacion.Services
{
    internal class ReporteService
    {
        private readonly IExportadorReporte _exportadorReporte;

        public ReporteService(IExportadorReporte exportadorReporte)
        {
            _exportadorReporte = exportadorReporte;
        }

        public string Generar(ReporteVenta reporte)
        {
            return _exportadorReporte.Exportar(reporte);
        }
    }
}
