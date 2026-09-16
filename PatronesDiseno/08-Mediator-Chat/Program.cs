using System;
using MediatorChat.Mediator;
using MediatorChat.Participants;

namespace MediatorChat
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SalaChat sala = new SalaChat();
            Usuario ana = new Usuario("Ana");
            Usuario bruno = new Usuario("Bruno");
            Usuario carla = new Usuario("Carla");

            sala.Registrar(ana);
            sala.Registrar(bruno);
            sala.Registrar(carla);

            Console.WriteLine("=== Mediator - Chat interno ===");
            ana.Enviar("Tenemos reunion a las 10.");
            Console.WriteLine();
            bruno.Enviar("Llevo el reporte de ventas.");

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
