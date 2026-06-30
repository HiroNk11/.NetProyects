namespace GeneradorReportes
{
    internal class ReporteService
    {
        private readonly IReporteFormatter _formatter;
        private readonly IReporteDestino _destino;

        public ReporteService(IReporteFormatter formatter, IReporteDestino destino)
        {
            _formatter = formatter;
            _destino = destino;
        }

        public void Generar(ReporteVentas reporte)
        {
            string contenido = _formatter.Formatear(reporte);
            _destino.Guardar(contenido);
        }
    }
}
