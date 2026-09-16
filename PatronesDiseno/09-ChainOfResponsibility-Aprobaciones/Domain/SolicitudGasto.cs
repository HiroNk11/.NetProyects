namespace ChainOfResponsibilityAprobaciones.Domain
{
    internal class SolicitudGasto
    {
        public SolicitudGasto(string solicitante, string concepto, decimal importe)
        {
            Solicitante = solicitante;
            Concepto = concepto;
            Importe = importe;
        }

        public string Solicitante { get; }

        public string Concepto { get; }

        public decimal Importe { get; }
    }
}
