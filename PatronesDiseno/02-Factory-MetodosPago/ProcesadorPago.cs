namespace FactoryMetodosPago
{
    internal class ProcesadorPago
    {
        public ResultadoPago Procesar(IMetodoPago metodoPago, decimal importe)
        {
            return metodoPago.Pagar(importe);
        }
    }
}
