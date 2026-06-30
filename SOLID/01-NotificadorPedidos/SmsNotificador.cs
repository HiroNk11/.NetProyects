using System;

namespace NotificadorPedidos
{
    internal class SmsNotificador : INotificador
    {
        public void Enviar(string destinatario, string mensaje)
        {
            Console.WriteLine($"SMS enviado a {destinatario}: {mensaje}");
        }
    }
}
