namespace StrategyDescuentos
{
    internal interface IEstrategiaDescuento
    {
        string Nombre { get; }
        decimal Calcular(decimal importe);
    }
}
