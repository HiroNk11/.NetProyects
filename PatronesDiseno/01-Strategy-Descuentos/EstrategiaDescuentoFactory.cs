namespace StrategyDescuentos
{
    internal static class EstrategiaDescuentoFactory
    {
        public static IEstrategiaDescuento Crear(string opcion)
        {
            switch (opcion)
            {
                case "1":
                    return new SinDescuento();
                case "2":
                    return new DescuentoPorcentaje(10);
                case "3":
                    return new DescuentoMontoFijo(1500);
                case "4":
                    return new DescuentoClienteFrecuente();
                default:
                    return new SinDescuento();
            }
        }
    }
}
