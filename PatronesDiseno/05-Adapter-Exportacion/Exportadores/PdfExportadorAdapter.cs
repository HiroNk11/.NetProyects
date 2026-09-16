using AdapterExportacion.Domain;
using AdapterExportacion.ExternalServices;

namespace AdapterExportacion.Exportadores
{
    internal class PdfExportadorAdapter : IExportadorReporte
    {
        private readonly PdfLibrary _pdfLibrary;

        public PdfExportadorAdapter(PdfLibrary pdfLibrary)
        {
            _pdfLibrary = pdfLibrary;
        }

        public string Exportar(ReporteVenta reporte)
        {
            string body = $"Ventas: {reporte.CantidadVentas}\nTotal facturado: ${reporte.TotalFacturado:0.00}";
            return _pdfLibrary.BuildDocument($"Reporte de ventas - {reporte.Periodo}", body);
        }
    }
}
