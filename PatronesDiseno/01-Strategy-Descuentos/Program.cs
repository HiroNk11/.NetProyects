using System;

namespace StrategyDescuentos
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== Strategy - Calculadora de descuentos ===");

            decimal subtotal = PedirImporte("Ingrese el subtotal de la compra: ");
            MostrarMenu();

            string opcion = Console.ReadLine();
            IEstrategiaDescuento estrategia = EstrategiaDescuentoFactory.Crear(opcion);
            CalculadoraDescuento calculadora = new CalculadoraDescuento(estrategia);

            Console.WriteLine();
            Console.WriteLine(calculadora.ObtenerResumen(subtotal));

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para salir...");
            Console.ReadKey();
        }

        private static void MostrarMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Seleccione una estrategia:");
            Console.WriteLine("1. Sin descuento");
            Console.WriteLine("2. Descuento del 10%");
            Console.WriteLine("3. Descuento fijo");
            Console.WriteLine("4. Cliente frecuente");
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
