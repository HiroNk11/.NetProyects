namespace SistemaFacturacion
{
    internal class ItemFactura
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public decimal CalcularSubtotal()
        {
            return Producto.PrecioUnitario * Cantidad;
        }

        public string ObtenerResumen()
        {
            return $"{Producto.Nombre} | Cantidad: {Cantidad} | Unitario: ${Producto.PrecioUnitario:0.00} | Subtotal: ${CalcularSubtotal():0.00}";
        }
    }
}
