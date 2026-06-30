namespace FactoryMetodosPago
{
    internal class PagoTransferencia : IMetodoPago
    {
        public string Nombre => "Transferencia";

        public ResultadoPago Pagar(decimal importe)
        {
            decimal descuento = importe * 0.03m;
            decimal total = importe - descuento;

            return new ResultadoPago(true, $"Pago por transferencia aprobado. Total con descuento: ${total:0.00}.");
        }
    }
}
