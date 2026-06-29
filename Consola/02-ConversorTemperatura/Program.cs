using System;

namespace ConversorTemperatura
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
                        ConvertirCelsiusAFahrenheit();
                        break;
                    case "2":
                        ConvertirFahrenheitACelsius();
                        break;
                    case "3":
                        ConvertirCelsiusAKelvin();
                        break;
                    case "4":
                        ConvertirKelvinACelsius();
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
            Console.WriteLine("=== Conversor de temperatura ===");
            Console.WriteLine("1. Celsius a Fahrenheit");
            Console.WriteLine("2. Fahrenheit a Celsius");
            Console.WriteLine("3. Celsius a Kelvin");
            Console.WriteLine("4. Kelvin a Celsius");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void ConvertirCelsiusAFahrenheit()
        {
            double celsius = PedirTemperatura("Ingrese la temperatura en Celsius: ");
            double fahrenheit = (celsius * 9 / 5) + 32;

            MostrarResultado(celsius, "C", fahrenheit, "F");
        }

        private static void ConvertirFahrenheitACelsius()
        {
            double fahrenheit = PedirTemperatura("Ingrese la temperatura en Fahrenheit: ");
            double celsius = (fahrenheit - 32) * 5 / 9;

            MostrarResultado(fahrenheit, "F", celsius, "C");
        }

        private static void ConvertirCelsiusAKelvin()
        {
            double celsius = PedirTemperatura("Ingrese la temperatura en Celsius: ");
            double kelvin = celsius + 273.15;

            MostrarResultado(celsius, "C", kelvin, "K");
        }

        private static void ConvertirKelvinACelsius()
        {
            double kelvin = PedirTemperatura("Ingrese la temperatura en Kelvin: ");

            if (kelvin < 0)
            {
                Console.WriteLine("La temperatura en Kelvin no puede ser menor a 0.");
                return;
            }

            double celsius = kelvin - 273.15;
            MostrarResultado(kelvin, "K", celsius, "C");
        }

        private static double PedirTemperatura(string mensaje)
        {
            double temperatura;
            bool esValida;

            do
            {
                Console.Write(mensaje);
                esValida = double.TryParse(Console.ReadLine(), out temperatura);

                if (!esValida)
                {
                    Console.WriteLine("Valor invalido. Ingrese un numero.");
                }
            }
            while (!esValida);

            return temperatura;
        }

        private static void MostrarResultado(double valorOriginal, string unidadOriginal, double resultado, string unidadResultado)
        {
            Console.WriteLine($"{valorOriginal:0.##} °{unidadOriginal} equivalen a {resultado:0.##} °{unidadResultado}.");
        }
    }
}
