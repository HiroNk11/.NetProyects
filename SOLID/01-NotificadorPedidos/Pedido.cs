namespace NotificadorPedidos
{
    internal class Pedido
    {
        public Pedido(int numero, string cliente, string contacto, decimal total)
        {
            Numero = numero;
            Cliente = cliente;
            Contacto = contacto;
            Total = total;
        }

        public int Numero { get; }
        public string Cliente { get; }
        public string Contacto { get; }
        public decimal Total { get; }
    }
}
