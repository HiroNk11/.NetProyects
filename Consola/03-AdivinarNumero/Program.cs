using System;

namespace AdivinarNumero
{
    internal class Program
    {
        private const int NumeroMinimo = 1;
        private const int NumeroMaximo = 100;
        private const int IntentosMaximos = 7;

        private static void Main(string[] args)
        {
            bool jugarOtraVez;

            do
            {
                JugarPartida();
                jugarOtraVez = PreguntarSiDeseaRepetir();
            }
            while (jugarOtraVez);

            Console.WriteLine("Gracias por jugar.");
        }

        private static void JugarPartida()
        {
            Random random = new Random();
            int numeroSecreto = random.Next(NumeroMinimo, NumeroMaximo + 1);
            bool acerto = false;

            Console.Clear();
            Console.WriteLine("=== Adivinar numero ===");
            Console.WriteLine($"Estoy pensando en un numero entre {NumeroMinimo} y {NumeroMaximo}.");
            Console.WriteLine($"Tenes {IntentosMaximos} intentos para adivinarlo.");
            Console.WriteLine();

            for (int intento = 1; intento <= IntentosMaximos; intento++)
            {
                int numeroIngresado = PedirNumero($"Intento {intento}/{IntentosMaximos}. Ingrese un numero: ");

                if (numeroIngresado == numeroSecreto)
                {
                    Console.WriteLine($"Correcto. Adivinaste el numero en {intento} intento(s).");
                    acerto = true;
                    break;
                }

                if (numeroIngresado < numeroSecreto)
                {
                    Console.WriteLine("El numero secreto es mayor.");
                }
                else
                {
                    Console.WriteLine("El numero secreto es menor.");
                }

                Console.WriteLine();
            }

            if (!acerto)
            {
                Console.WriteLine($"Se terminaron los intentos. El numero secreto era {numeroSecreto}.");
            }
        }

        private static int PedirNumero(string mensaje)
        {
            int numero;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out numero);

                if (!esValido)
                {
                    Console.WriteLine("Valor invalido. Ingrese un numero entero.");
                    continue;
                }

                if (numero < NumeroMinimo || numero > NumeroMaximo)
                {
                    Console.WriteLine($"El numero debe estar entre {NumeroMinimo} y {NumeroMaximo}.");
                    esValido = false;
                }
            }
            while (!esValido);

            return numero;
        }

        private static bool PreguntarSiDeseaRepetir()
        {
            string respuesta;

            do
            {
                Console.WriteLine();
                Console.Write("Desea jugar otra vez? (s/n): ");
                respuesta = Console.ReadLine().ToLower();

                if (respuesta == "s")
                {
                    return true;
                }

                if (respuesta == "n")
                {
                    return false;
                }

                Console.WriteLine("Respuesta invalida. Ingrese 's' o 'n'.");
            }
            while (true);
        }
    }
}
