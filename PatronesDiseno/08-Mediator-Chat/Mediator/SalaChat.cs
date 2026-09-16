using System;
using System.Collections.Generic;
using MediatorChat.Participants;

namespace MediatorChat.Mediator
{
    internal class SalaChat : IChatMediator
    {
        private readonly List<Usuario> _usuarios = new List<Usuario>();

        public void Registrar(Usuario usuario)
        {
            if (!_usuarios.Contains(usuario))
            {
                _usuarios.Add(usuario);
                usuario.AsignarMediator(this);
            }
        }

        public void Enviar(string mensaje, Usuario remitente)
        {
            Console.WriteLine($"[{remitente.Nombre}] envia: {mensaje}");

            foreach (Usuario usuario in _usuarios)
            {
                if (usuario != remitente)
                {
                    usuario.Recibir(mensaje, remitente.Nombre);
                }
            }
        }
    }
}
