using System;

namespace NotificadorPedidos
{
    internal class WhatsAppNotificador : INotificador
    {
        public void Enviar(string destinatario, string mensaje)
        {
            Console.WriteLine($"WhatsApp enviado a {destinatario}: {mensaje}");
        }
    }
}
