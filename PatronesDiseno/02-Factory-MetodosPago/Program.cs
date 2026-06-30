using System;

namespace FactoryMetodosPago
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== Factory - Metodos de pago ===");

            decimal importe = PedirImporte("Ingrese el importe a pagar: ");
            MostrarMenu();

            string opcion = Console.ReadLine();
            IMetodoPago metodoPago = MetodoPagoFactory.Crear(opcion);
            ProcesadorPago procesador = new ProcesadorPago();
            ResultadoPago resultado = procesador.Procesar(metodoPago, importe);

            Console.WriteLine();
            Console.WriteLine($"Metodo seleccionado: {metodoPago.Nombre}");
            Console.WriteLine(resultado.Mensaje);

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para salir...");
            Console.ReadKey();
        }

        private static void MostrarMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Seleccione metodo de pago:");
            Console.WriteLine("1. Efectivo");
            Console.WriteLine("2. Tarjeta");
            Console.WriteLine("3. Transferencia");
            Console.Write("Opcion: ");
        }

        private static decimal PedirImporte(string mensaje)
        {
            decimal importe;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = decimal.TryParse(Console.ReadLine(), out importe);

                if (!esValido || importe <= 0)
                {
                    Console.WriteLine("Ingrese un importe mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return importe;
        }
    }
}
