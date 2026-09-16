using DecoratorNotificaciones.Domain;

namespace DecoratorNotificaciones.Notificadores
{
    internal interface INotificador
    {
        void Enviar(Mensaje mensaje);
    }
}
