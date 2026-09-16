namespace AdapterExportacion.Domain
{
    internal class ReporteVenta
    {
        public ReporteVenta(string periodo, int cantidadVentas, decimal totalFacturado)
        {
            Periodo = periodo;
            CantidadVentas = cantidadVentas;
            TotalFacturado = totalFacturado;
        }

        public string Periodo { get; }

        public int CantidadVentas { get; }

        public decimal TotalFacturado { get; }
    }
}
