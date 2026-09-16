using ProcesadorPagosSolid.Domain;

namespace ProcesadorPagosSolid.Payments
{
    internal class BilleteraVirtualGateway : IPaymentGateway
    {
        public string Nombre => "Billetera virtual";

        public ResultadoPago Procesar(OrdenCompra ordenCompra)
        {
            return ordenCompra.Total <= 250000
                ? new ResultadoPago(true, "Pago aprobado desde billetera virtual.")
                : new ResultadoPago(false, "La billetera virtual no permite pagos mayores a $250000.");
        }
    }
}
