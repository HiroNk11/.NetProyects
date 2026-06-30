namespace GeneradorReportes
{
    internal class Venta
    {
        public Venta(string producto, int cantidad, decimal precioUnitario)
        {
            Producto = producto;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }

        public string Producto { get; }
        public int Cantidad { get; }
        public decimal PrecioUnitario { get; }
        public decimal Total => Cantidad * PrecioUnitario;
    }
}
