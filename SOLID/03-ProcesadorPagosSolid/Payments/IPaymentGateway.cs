using ProcesadorPagosSolid.Domain;

namespace ProcesadorPagosSolid.Payments
{
    internal interface IPaymentGateway
    {
        string Nombre { get; }

        ResultadoPago Procesar(OrdenCompra ordenCompra);
    }
}
