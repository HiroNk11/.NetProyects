namespace NotificadorPedidos
{
    internal static class NotificadorFactory
    {
        public static INotificador Crear(string opcion)
        {
            switch (opcion)
            {
                case "1":
                    return new EmailNotificador();
                case "2":
                    return new SmsNotificador();
                case "3":
                    return new WhatsAppNotificador();
                default:
                    return new EmailNotificador();
            }
        }
    }
}
