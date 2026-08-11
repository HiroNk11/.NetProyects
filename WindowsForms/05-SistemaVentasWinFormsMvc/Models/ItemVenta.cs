namespace SistemaVentasWinFormsMvc.Models
{
    internal class ItemVenta
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
        public string ProductoNombre => Producto.Nombre;
    }
}
