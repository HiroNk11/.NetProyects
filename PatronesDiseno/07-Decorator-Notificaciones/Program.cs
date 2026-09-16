using System;
using DecoratorNotificaciones.Decorators;
using DecoratorNotificaciones.Domain;
using DecoratorNotificaciones.Notificadores;

namespace DecoratorNotificaciones
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Mensaje mensaje = new Mensaje(
                "cliente@empresa.com",
                "Pedido confirmado",
                "Su pedido fue registrado y se encuentra en preparacion.");

            INotificador notificador = new EmailNotificador();
            notificador = new SmsNotificadorDecorator(notificador);
            notificador = new AuditoriaNotificadorDecorator(notificador);
            notificador = new PrioridadNotificadorDecorator(notificador);

            Console.WriteLine("=== Decorator - Notificaciones ===");
            notificador.Enviar(mensaje);

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
