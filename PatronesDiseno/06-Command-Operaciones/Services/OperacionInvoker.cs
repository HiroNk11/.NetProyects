using System.Collections.Generic;
using CommandOperaciones.Commands;

namespace CommandOperaciones.Services
{
    internal class OperacionInvoker
    {
        private readonly Stack<ICommand> _historial = new Stack<ICommand>();

        public bool Ejecutar(ICommand command)
        {
            bool resultado = command.Ejecutar();

            if (resultado)
            {
                _historial.Push(command);
            }

            return resultado;
        }

        public bool DeshacerUltima()
        {
            if (_historial.Count == 0)
            {
                return false;
            }

            ICommand command = _historial.Pop();
            command.Deshacer();
            return true;
        }
    }
}
