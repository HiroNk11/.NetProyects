namespace StrategyDescuentos
{
    internal class DescuentoClienteFrecuente : IEstrategiaDescuento
    {
        public string Nombre => "Cliente frecuente";

        public decimal Calcular(decimal importe)
        {
            decimal porcentaje = importe >= 20000 ? 15 : 10;
            return importe * porcentaje / 100;
        }
    }
}
