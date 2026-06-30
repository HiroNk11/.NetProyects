namespace FactoryMetodosPago
{
    internal class PagoTarjeta : IMetodoPago
    {
        public string Nombre => "Tarjeta";

        public ResultadoPago Pagar(decimal importe)
        {
            decimal recargo = importe * 0.05m;
            decimal total = importe + recargo;

            return new ResultadoPago(true, $"Pago con tarjeta aprobado. Total con recargo: ${total:0.00}.");
        }
    }
}
