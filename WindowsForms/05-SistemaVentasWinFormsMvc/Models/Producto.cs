namespace SistemaVentasWinFormsMvc.Models
{
    internal class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public override string ToString()
        {
            return $"{Nombre} (${Precio:0.00})";
        }
    }
}
