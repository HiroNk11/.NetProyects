using AdapterExportacion.Domain;

namespace AdapterExportacion.Exportadores
{
    internal interface IExportadorReporte
    {
        string Exportar(ReporteVenta reporte);
    }
}
