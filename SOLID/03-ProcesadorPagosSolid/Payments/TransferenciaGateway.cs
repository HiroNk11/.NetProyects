using ProcesadorPagosSolid.Domain;

namespace ProcesadorPagosSolid.Payments
{
    internal class TransferenciaGateway : IPaymentGateway
    {
        public string Nombre => "Transferencia bancaria";

        public ResultadoPago Procesar(OrdenCompra ordenCompra)
        {
            return new ResultadoPago(true, $"Se genero una orden de transferencia para {ordenCompra.Cliente}.");
        }
    }
}
