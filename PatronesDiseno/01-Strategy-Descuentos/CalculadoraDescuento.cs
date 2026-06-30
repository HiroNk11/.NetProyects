namespace StrategyDescuentos
{
    internal class CalculadoraDescuento
    {
        private readonly IEstrategiaDescuento _estrategia;

        public CalculadoraDescuento(IEstrategiaDescuento estrategia)
        {
            _estrategia = estrategia;
        }

        public decimal CalcularTotal(decimal subtotal)
        {
            decimal descuento = _estrategia.Calcular(subtotal);
            return subtotal - descuento;
        }

        public string ObtenerResumen(decimal subtotal)
        {
            decimal descuento = _estrategia.Calcular(subtotal);
            decimal total = subtotal - descuento;

            return $"Estrategia: {_estrategia.Nombre}\nSubtotal: ${subtotal:0.00}\nDescuento: ${descuento:0.00}\nTotal: ${total:0.00}";
        }
    }
}
