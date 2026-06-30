namespace FactoryMetodosPago
{
    internal class PagoEfectivo : IMetodoPago
    {
        public string Nombre => "Efectivo";

        public ResultadoPago Pagar(decimal importe)
        {
            return new ResultadoPago(true, $"Pago en efectivo aprobado por ${importe:0.00}.");
        }
    }
}
