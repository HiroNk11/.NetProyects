using MediatorChat.Participants;

namespace MediatorChat.Mediator
{
    internal interface IChatMediator
    {
        void Registrar(Usuario usuario);

        void Enviar(string mensaje, Usuario remitente);
    }
}
