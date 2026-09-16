using System;
using DecoratorNotificaciones.Domain;
using DecoratorNotificaciones.Notificadores;

namespace DecoratorNotificaciones.Decorators
{
    internal class PrioridadNotificadorDecorator : NotificadorDecorator
    {
        public PrioridadNotificadorDecorator(INotificador notificador)
            : base(notificador)
        {
        }

        public override void Enviar(Mensaje mensaje)
        {
            Console.WriteLine("[PRIORIDAD] El mensaje se marco como urgente.");
            base.Enviar(mensaje);
        }
    }
}
