using System;

namespace NotificadorPedidos
{
    internal class EmailNotificador : INotificador
    {
        public void Enviar(string destinatario, string mensaje)
        {
            Console.WriteLine($"Email enviado a {destinatario}: {mensaje}");
        }
    }
}
