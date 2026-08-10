using System;
using System.Collections.Generic;
using System.Linq;

namespace BancoMultiplesCuentas
{
    internal class Program
    {
        private static readonly List<Cuenta> Cuentas = new List<Cuenta>();
        private static int proximoNumero = 1001;

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
                        CrearCuenta();
                        break;
                    case "2":
                        ListarCuentas();
                        break;
                    case "3":
                        Depositar();
                        break;
                    case "4":
                        Extraer();
                        break;
                    case "5":
                        Transferir();
                        break;
                    case "6":
                        VerMovimientos();
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
            Console.WriteLine("=== Banco con multiples cuentas ===");
            Console.WriteLine("1. Crear cuenta");
            Console.WriteLine("2. Listar cuentas");
            Console.WriteLine("3. Depositar");
            Console.WriteLine("4. Extraer");
            Console.WriteLine("5. Transferir");
            Console.WriteLine("6. Ver movimientos");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void CrearCuenta()
        {
            string titular = PedirTexto("Ingrese el titular: ");
            decimal saldoInicial = PedirImportePermitidoCero("Ingrese el saldo inicial: ");

            Cuenta cuenta = new Cuenta(proximoNumero, titular, saldoInicial);
            Cuentas.Add(cuenta);
            proximoNumero++;

            Console.WriteLine("Cuenta creada correctamente.");
            Console.WriteLine(cuenta.ObtenerResumen());
        }

        private static void ListarCuentas()
        {
            Console.WriteLine();

            if (Cuentas.Count == 0)
            {
                Console.WriteLine("No hay cuentas registradas.");
                return;
            }

            foreach (Cuenta cuenta in Cuentas)
            {
                Console.WriteLine(cuenta.ObtenerResumen());
            }
        }

        private static void Depositar()
        {
            Cuenta cuenta = SeleccionarCuenta("Ingrese el numero de cuenta: ");

            if (cuenta == null)
            {
                return;
            }

            decimal importe = PedirImporte("Ingrese el importe a depositar: ");
            cuenta.Depositar(importe);
            Console.WriteLine("Deposito realizado correctamente.");
        }

        private static void Extraer()
        {
            Cuenta cuenta = SeleccionarCuenta("Ingrese el numero de cuenta: ");

            if (cuenta == null)
            {
                return;
            }

            decimal importe = PedirImporte("Ingrese el importe a extraer: ");

            if (!cuenta.Extraer(importe))
            {
                Console.WriteLine("Saldo insuficiente.");
                return;
            }

            Console.WriteLine("Extraccion realizada correctamente.");
        }

        private static void Transferir()
        {
            Cuenta origen = SeleccionarCuenta("Ingrese la cuenta origen: ");

            if (origen == null)
            {
                return;
            }

            Cuenta destino = SeleccionarCuenta("Ingrese la cuenta destino: ");

            if (destino == null)
            {
                return;
            }

            if (origen.Numero == destino.Numero)
            {
                Console.WriteLine("La cuenta origen y destino no pueden ser la misma.");
                return;
            }

            decimal importe = PedirImporte("Ingrese el importe a transferir: ");

            if (!origen.TransferirA(destino, importe))
            {
                Console.WriteLine("Saldo insuficiente para transferir.");
                return;
            }

            Console.WriteLine("Transferencia realizada correctamente.");
        }

        private static void VerMovimientos()
        {
            Cuenta cuenta = SeleccionarCuenta("Ingrese el numero de cuenta: ");

            if (cuenta == null)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine(cuenta.ObtenerResumen());
            Console.WriteLine("Movimientos:");

            foreach (Movimiento movimiento in cuenta.Movimientos)
            {
                Console.WriteLine(movimiento.ObtenerResumen());
            }
        }

        private static Cuenta SeleccionarCuenta(string mensaje)
        {
            if (Cuentas.Count == 0)
            {
                Console.WriteLine("No hay cuentas registradas.");
                return null;
            }

            ListarCuentas();
            int numero = PedirEntero(mensaje);
            Cuenta cuenta = Cuentas.FirstOrDefault(item => item.Numero == numero);

            if (cuenta == null)
            {
                Console.WriteLine("No se encontro una cuenta con ese numero.");
            }

            return cuenta;
        }

        private static string PedirTexto(string mensaje)
        {
            string texto;

            do
            {
                Console.Write(mensaje);
                texto = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    Console.WriteLine("El valor no puede estar vacio.");
                }
            }
            while (string.IsNullOrWhiteSpace(texto));

            return texto.Trim();
        }

        private static int PedirEntero(string mensaje)
        {
            int numero;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out numero);

                if (!esValido || numero <= 0)
                {
                    Console.WriteLine("Ingrese un numero entero mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return numero;
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

        private static decimal PedirImportePermitidoCero(string mensaje)
        {
            decimal importe;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = decimal.TryParse(Console.ReadLine(), out importe);

                if (!esValido || importe < 0)
                {
                    Console.WriteLine("Ingrese un importe igual o mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return importe;
        }
    }
}
