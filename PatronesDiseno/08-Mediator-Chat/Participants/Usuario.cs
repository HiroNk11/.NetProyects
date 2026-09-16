using System;
using MediatorChat.Mediator;

namespace MediatorChat.Participants
{
    internal class Usuario
    {
        private IChatMediator _mediator;

        public Usuario(string nombre)
        {
            Nombre = nombre;
        }

        public string Nombre { get; }

        public void AsignarMediator(IChatMediator mediator)
        {
            _mediator = mediator;
        }

        public void Enviar(string mensaje)
        {
            _mediator.Enviar(mensaje, this);
        }

        public void Recibir(string mensaje, string remitente)
        {
            Console.WriteLine($"  {Nombre} recibe de {remitente}: {mensaje}");
        }
    }
}
