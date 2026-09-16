using CommandOperaciones.Domain;

namespace CommandOperaciones.Commands
{
    internal class DepositarCommand : ICommand
    {
        private readonly CuentaBancaria _cuenta;
        private readonly decimal _importe;

        public DepositarCommand(CuentaBancaria cuenta, decimal importe)
        {
            _cuenta = cuenta;
            _importe = importe;
        }

        public string Nombre => $"Deposito de ${_importe:0.00}";

        public bool Ejecutar()
        {
            _cuenta.Depositar(_importe);
            return true;
        }

        public void Deshacer()
        {
            _cuenta.Extraer(_importe);
        }
    }
}
