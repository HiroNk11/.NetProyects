namespace FactoryMetodosPago
{
    internal static class MetodoPagoFactory
    {
        public static IMetodoPago Crear(string opcion)
        {
            switch (opcion)
            {
                case "1":
                    return new PagoEfectivo();
                case "2":
                    return new PagoTarjeta();
                case "3":
                    return new PagoTransferencia();
                default:
                    return new PagoEfectivo();
            }
        }
    }
}
