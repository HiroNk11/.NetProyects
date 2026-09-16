using DecoratorNotificaciones.Domain;
using DecoratorNotificaciones.Notificadores;

namespace DecoratorNotificaciones.Decorators
{
    internal abstract class NotificadorDecorator : INotificador
    {
        private readonly INotificador _notificador;

        protected NotificadorDecorator(INotificador notificador)
        {
            _notificador = notificador;
        }

        public virtual void Enviar(Mensaje mensaje)
        {
            _notificador.Enviar(mensaje);
        }
    }
}
