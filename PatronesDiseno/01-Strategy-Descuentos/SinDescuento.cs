namespace StrategyDescuentos
{
    internal class SinDescuento : IEstrategiaDescuento
    {
        public string Nombre => "Sin descuento";

        public decimal Calcular(decimal importe)
        {
            return 0;
        }
    }
}
