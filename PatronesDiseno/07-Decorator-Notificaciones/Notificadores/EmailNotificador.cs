using System;
using DecoratorNotificaciones.Domain;

namespace DecoratorNotificaciones.Notificadores
{
    internal class EmailNotificador : INotificador
    {
        public void Enviar(Mensaje mensaje)
        {
            Console.WriteLine($"[EMAIL] Para: {mensaje.Destinatario} | {mensaje.Asunto}");
            Console.WriteLine(mensaje.Contenido);
        }
    }
}
