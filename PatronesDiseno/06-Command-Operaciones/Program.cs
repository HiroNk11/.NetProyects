using System;
using CommandOperaciones.Commands;
using CommandOperaciones.Domain;
using CommandOperaciones.Services;

namespace CommandOperaciones
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CuentaBancaria cuenta = new CuentaBancaria("Nicolas", 100000);
            OperacionInvoker invoker = new OperacionInvoker();

            Console.WriteLine("=== Command - Operaciones bancarias ===");
            MostrarSaldo(cuenta);

            Ejecutar(invoker, new DepositarCommand(cuenta, 25000), cuenta);
            Ejecutar(invoker, new ExtraerCommand(cuenta, 40000), cuenta);
            Ejecutar(invoker, new ExtraerCommand(cuenta, 120000), cuenta);

            Console.WriteLine();
            Console.WriteLine("Deshaciendo ultima operacion valida...");
            invoker.DeshacerUltima();
            MostrarSaldo(cuenta);

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }

        private static void Ejecutar(OperacionInvoker invoker, ICommand command, CuentaBancaria cuenta)
        {
            Console.WriteLine();
            Console.WriteLine(command.Nombre);
            Console.WriteLine(invoker.Ejecutar(command) ? "Operacion realizada." : "Operacion rechazada.");
            MostrarSaldo(cuenta);
        }

        private static void MostrarSaldo(CuentaBancaria cuenta)
        {
            Console.WriteLine($"{cuenta.Titular} | Saldo: ${cuenta.Saldo:0.00}");
        }
    }
}
