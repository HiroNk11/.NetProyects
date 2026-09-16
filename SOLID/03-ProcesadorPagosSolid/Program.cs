using System;
using System.Collections.Generic;
using ProcesadorPagosSolid.Domain;
using ProcesadorPagosSolid.Payments;
using ProcesadorPagosSolid.Services;

namespace ProcesadorPagosSolid
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            OrdenCompra orden = new OrdenCompra(1001, "Ana Gomez", 310000);
            List<IPaymentGateway> gateways = new List<IPaymentGateway>
            {
                new TarjetaCreditoGateway(),
                new TransferenciaGateway(),
                new BilleteraVirtualGateway()
            };

            Console.WriteLine("=== SOLID - Procesador de pagos ===");
            Console.WriteLine($"Orden {orden.Numero} | Cliente: {orden.Cliente} | Total: ${orden.Total:0.00}");
            Console.WriteLine();

            foreach (IPaymentGateway gateway in gateways)
            {
                CheckoutService checkoutService = new CheckoutService(gateway);
                ResultadoPago resultado = checkoutService.Pagar(orden);

                Console.WriteLine($"{gateway.Nombre}: {(resultado.Aprobado ? "Aprobado" : "Rechazado")}");
                Console.WriteLine(resultado.Mensaje);
                Console.WriteLine();
            }

            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
