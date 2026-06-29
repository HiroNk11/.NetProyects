using System;

namespace CajeroAutomatico
{
    internal class Movimiento
    {
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public decimal Importe { get; set; }
        public decimal SaldoPosterior { get; set; }

        public string ObtenerResumen()
        {
            return $"{Fecha:dd/MM/yyyy HH:mm} | {Tipo} | Importe: ${Importe:0.00} | Saldo: ${SaldoPosterior:0.00}";
        }
    }
}
