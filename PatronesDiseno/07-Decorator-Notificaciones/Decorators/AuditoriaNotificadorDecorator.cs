using System;
using DecoratorNotificaciones.Domain;
using DecoratorNotificaciones.Notificadores;

namespace DecoratorNotificaciones.Decorators
{
    internal class AuditoriaNotificadorDecorator : NotificadorDecorator
    {
        public AuditoriaNotificadorDecorator(INotificador notificador)
            : base(notificador)
        {
        }

        public override void Enviar(Mensaje mensaje)
        {
            base.Enviar(mensaje);
            Console.WriteLine($"[AUDITORIA] Notificacion registrada para {mensaje.Destinatario}.");
        }
    }
}
