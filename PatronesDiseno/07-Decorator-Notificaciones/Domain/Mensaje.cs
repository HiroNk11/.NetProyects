namespace DecoratorNotificaciones.Domain
{
    internal class Mensaje
    {
        public Mensaje(string destinatario, string asunto, string contenido)
        {
            Destinatario = destinatario;
            Asunto = asunto;
            Contenido = contenido;
        }

        public string Destinatario { get; }

        public string Asunto { get; }

        public string Contenido { get; }
    }
}
