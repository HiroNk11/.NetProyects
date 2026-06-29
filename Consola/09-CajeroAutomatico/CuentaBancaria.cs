using System;
using System.Collections.Generic;

namespace CajeroAutomatico
{
    internal class CuentaBancaria
    {
        public string Titular { get; set; }
        public decimal Saldo { get; private set; }
        public List<Movimiento> Movimientos { get; } = new List<Movimiento>();

        public CuentaBancaria(string titular, decimal saldoInicial)
        {
            Titular = titular;
            Saldo = saldoInicial;
            RegistrarMovimiento("Saldo inicial", saldoInicial);
        }

        public void Depositar(decimal importe)
        {
            Saldo += importe;
            RegistrarMovimiento("Deposito", importe);
        }

        public bool Extraer(decimal importe)
        {
            if (importe > Saldo)
            {
                return false;
            }

            Saldo -= importe;
            RegistrarMovimiento("Extraccion", importe);
            return true;
        }

        private void RegistrarMovimiento(string tipo, decimal importe)
        {
            Movimientos.Add(new Movimiento
            {
                Fecha = DateTime.Now,
                Tipo = tipo,
                Importe = importe,
                SaldoPosterior = Saldo
            });
        }
    }
}
