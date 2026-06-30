namespace NotificadorPedidos
{
    internal interface INotificador
    {
        void Enviar(string destinatario, string mensaje);
    }
}
