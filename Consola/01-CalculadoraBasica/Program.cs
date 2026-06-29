using System;

namespace CalculadoraBasica
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        EjecutarOperacion("suma");
                        break;
                    case "2":
                        EjecutarOperacion("resta");
                        break;
                    case "3":
                        EjecutarOperacion("multiplicacion");
                        break;
                    case "4":
                        EjecutarOperacion("division");
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opcion invalida. Intente nuevamente.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Programa finalizado.");
        }

        private static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Calculadora basica ===");
            Console.WriteLine("1. Sumar");
            Console.WriteLine("2. Restar");
            Console.WriteLine("3. Multiplicar");
            Console.WriteLine("4. Dividir");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void EjecutarOperacion(string operacion)
        {
            Console.WriteLine();

            double primerNumero = PedirNumero("Ingrese el primer numero: ");
            double segundoNumero = PedirNumero("Ingrese el segundo numero: ");

            if (operacion == "division" && segundoNumero == 0)
            {
                Console.WriteLine("No se puede dividir por cero.");
                return;
            }

            double resultado = Calcular(primerNumero, segundoNumero, operacion);
            Console.WriteLine($"Resultado: {resultado}");
        }

        private static double PedirNumero(string mensaje)
        {
            double numero;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = double.TryParse(Console.ReadLine(), out numero);

                if (!esValido)
                {
                    Console.WriteLine("Valor invalido. Ingrese un numero.");
                }
            }
            while (!esValido);

            return numero;
        }

        private static double Calcular(double primerNumero, double segundoNumero, string operacion)
        {
            switch (operacion)
            {
                case "suma":
                    return primerNumero + segundoNumero;
                case "resta":
                    return primerNumero - segundoNumero;
                case "multiplicacion":
                    return primerNumero * segundoNumero;
                case "division":
                    return primerNumero / segundoNumero;
                default:
                    return 0;
            }
        }
    }
}
