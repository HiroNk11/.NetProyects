using ProcesadorPagosSolid.Domain;

namespace ProcesadorPagosSolid.Payments
{
    internal class TarjetaCreditoGateway : IPaymentGateway
    {
        public string Nombre => "Tarjeta de credito";

        public ResultadoPago Procesar(OrdenCompra ordenCompra)
        {
            return new ResultadoPago(true, $"Pago aprobado con tarjeta por ${ordenCompra.Total:0.00}.");
        }
    }
}
