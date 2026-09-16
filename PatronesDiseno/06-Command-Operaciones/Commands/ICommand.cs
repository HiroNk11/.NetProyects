namespace CommandOperaciones.Commands
{
    internal interface ICommand
    {
        string Nombre { get; }

        bool Ejecutar();

        void Deshacer();
    }
}
