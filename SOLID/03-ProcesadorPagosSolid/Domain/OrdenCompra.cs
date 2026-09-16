namespace ProcesadorPagosSolid.Domain
{
    internal class OrdenCompra
    {
        public OrdenCompra(int numero, string cliente, decimal total)
        {
            Numero = numero;
            Cliente = cliente;
            Total = total;
        }

        public int Numero { get; }

        public string Cliente { get; }

        public decimal Total { get; }
    }
}
