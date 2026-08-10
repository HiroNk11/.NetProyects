namespace RegistroVentas
{
    internal class ProductoVenta
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal CalcularSubtotal()
        {
            return Cantidad * PrecioUnitario;
        }

        public string ObtenerResumen()
        {
            return $"{Nombre} | Cantidad: {Cantidad} | Precio: ${PrecioUnitario:0.00} | Subtotal: ${CalcularSubtotal():0.00}";
        }
    }
}
