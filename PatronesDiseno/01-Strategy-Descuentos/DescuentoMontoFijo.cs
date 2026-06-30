using System;

namespace StrategyDescuentos
{
    internal class DescuentoMontoFijo : IEstrategiaDescuento
    {
        private readonly decimal _monto;

        public DescuentoMontoFijo(decimal monto)
        {
            _monto = monto;
        }

        public string Nombre => $"Descuento fijo de ${_monto:0.00}";

        public decimal Calcular(decimal importe)
        {
            return Math.Min(_monto, importe);
        }
    }
}
