using System;
using DecoratorNotificaciones.Domain;
using DecoratorNotificaciones.Notificadores;

namespace DecoratorNotificaciones.Decorators
{
    internal class SmsNotificadorDecorator : NotificadorDecorator
    {
        public SmsNotificadorDecorator(INotificador notificador)
            : base(notificador)
        {
        }

        public override void Enviar(Mensaje mensaje)
        {
            base.Enviar(mensaje);
            Console.WriteLine($"[SMS] Aviso enviado a {mensaje.Destinatario}.");
        }
    }
}
