using ProcesadorPagosSolid.Domain;
using ProcesadorPagosSolid.Payments;

namespace ProcesadorPagosSolid.Services
{
    internal class CheckoutService
    {
        private readonly IPaymentGateway _paymentGateway;

        public CheckoutService(IPaymentGateway paymentGateway)
        {
            _paymentGateway = paymentGateway;
        }

        public ResultadoPago Pagar(OrdenCompra ordenCompra)
        {
            return _paymentGateway.Procesar(ordenCompra);
        }
    }
}
