using CommandOperaciones.Domain;

namespace CommandOperaciones.Commands
{
    internal class ExtraerCommand : ICommand
    {
        private readonly CuentaBancaria _cuenta;
        private readonly decimal _importe;

        public ExtraerCommand(CuentaBancaria cuenta, decimal importe)
        {
            _cuenta = cuenta;
            _importe = importe;
        }

        public string Nombre => $"Extraccion de ${_importe:0.00}";

        public bool Ejecutar()
        {
            return _cuenta.Extraer(_importe);
        }

        public void Deshacer()
        {
            _cuenta.Depositar(_importe);
        }
    }
}
