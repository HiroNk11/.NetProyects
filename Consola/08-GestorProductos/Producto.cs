namespace GestorProductos
{
    internal class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public decimal CalcularValorTotal()
        {
            return Precio * Stock;
        }

        public string ObtenerResumen()
        {
            return $"{Id}. {Nombre} | Precio: ${Precio:0.00} | Stock: {Stock} | Total: ${CalcularValorTotal():0.00}";
        }
    }
}
