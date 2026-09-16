using AdapterExportacion.Domain;
using AdapterExportacion.ExternalServices;

namespace AdapterExportacion.Exportadores
{
    internal class JsonExportadorAdapter : IExportadorReporte
    {
        private readonly JsonLibrary _jsonLibrary;

        public JsonExportadorAdapter(JsonLibrary jsonLibrary)
        {
            _jsonLibrary = jsonLibrary;
        }

        public string Exportar(ReporteVenta reporte)
        {
            return _jsonLibrary.ConvertToJson(reporte.Periodo, reporte.CantidadVentas, reporte.TotalFacturado);
        }
    }
}
