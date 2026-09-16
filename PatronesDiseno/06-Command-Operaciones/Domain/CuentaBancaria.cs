namespace CommandOperaciones.Domain
{
    internal class CuentaBancaria
    {
        public CuentaBancaria(string titular, decimal saldoInicial)
        {
            Titular = titular;
            Saldo = saldoInicial;
        }

        public string Titular { get; }

        public decimal Saldo { get; private set; }

        public void Depositar(decimal importe)
        {
            Saldo += importe;
        }

        public bool Extraer(decimal importe)
        {
            if (importe > Saldo)
            {
                return false;
            }

            Saldo -= importe;
            return true;
        }
    }
}
