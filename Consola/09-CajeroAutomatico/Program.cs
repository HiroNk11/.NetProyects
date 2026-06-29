using System;

namespace CajeroAutomatico
{
    internal class Program
    {
        private static readonly CuentaBancaria Cuenta = new CuentaBancaria("Usuario Demo", 10000);

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
                        ConsultarSaldo();
                        break;
                    case "2":
                        DepositarDinero();
                        break;
                    case "3":
                        ExtraerDinero();
                        break;
                    case "4":
                        MostrarHistorial();
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
            Console.WriteLine("=== Cajero automatico ===");
            Console.WriteLine($"Titular: {Cuenta.Titular}");
            Console.WriteLine("1. Consultar saldo");
            Console.WriteLine("2. Depositar dinero");
            Console.WriteLine("3. Extraer dinero");
            Console.WriteLine("4. Ver historial de movimientos");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void ConsultarSaldo()
        {
            Console.WriteLine();
            Console.WriteLine($"Saldo disponible: ${Cuenta.Saldo:0.00}");
        }

        private static void DepositarDinero()
        {
            decimal importe = PedirImporte("Ingrese el importe a depositar: ");
            Cuenta.Depositar(importe);

            Console.WriteLine("Deposito realizado correctamente.");
            Console.WriteLine($"Nuevo saldo: ${Cuenta.Saldo:0.00}");
        }

        private static void ExtraerDinero()
        {
            decimal importe = PedirImporte("Ingrese el importe a extraer: ");
            bool extraccionExitosa = Cuenta.Extraer(importe);

            if (!extraccionExitosa)
            {
                Console.WriteLine("Saldo insuficiente para realizar la extraccion.");
                return;
            }

            Console.WriteLine("Extraccion realizada correctamente.");
            Console.WriteLine($"Nuevo saldo: ${Cuenta.Saldo:0.00}");
        }

        private static void MostrarHistorial()
        {
            Console.WriteLine();
            Console.WriteLine("Historial de movimientos:");

            foreach (Movimiento movimiento in Cuenta.Movimientos)
            {
                Console.WriteLine(movimiento.ObtenerResumen());
            }
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
