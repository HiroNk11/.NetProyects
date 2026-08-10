using System;
using System.Collections.Generic;

namespace BancoMultiplesCuentas
{
    internal class Cuenta
    {
        public int Numero { get; set; }
        public string Titular { get; set; }
        public decimal Saldo { get; private set; }
        public List<Movimiento> Movimientos { get; } = new List<Movimiento>();

        public Cuenta(int numero, string titular, decimal saldoInicial)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldoInicial;
            RegistrarMovimiento("Apertura", saldoInicial);
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

        public bool TransferirA(Cuenta destino, decimal importe)
        {
            if (!Extraer(importe))
            {
                return false;
            }

            destino.Depositar(importe);
            RegistrarMovimiento($"Transferencia enviada a {destino.Numero}", importe);
            destino.RegistrarMovimiento($"Transferencia recibida de {Numero}", importe);
            return true;
        }

        public string ObtenerResumen()
        {
            return $"{Numero}. {Titular} | Saldo: ${Saldo:0.00}";
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
