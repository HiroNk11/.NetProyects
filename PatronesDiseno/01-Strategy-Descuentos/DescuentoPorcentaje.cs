namespace StrategyDescuentos
{
    internal class DescuentoPorcentaje : IEstrategiaDescuento
    {
        private readonly decimal _porcentaje;

        public DescuentoPorcentaje(decimal porcentaje)
        {
            _porcentaje = porcentaje;
        }

        public string Nombre => $"Descuento del {_porcentaje:0}%";

        public decimal Calcular(decimal importe)
        {
            return importe * _porcentaje / 100;
        }
    }
}
