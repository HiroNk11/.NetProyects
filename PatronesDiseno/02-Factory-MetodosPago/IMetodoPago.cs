namespace FactoryMetodosPago
{
    internal interface IMetodoPago
    {
        string Nombre { get; }
        ResultadoPago Pagar(decimal importe);
    }
}
