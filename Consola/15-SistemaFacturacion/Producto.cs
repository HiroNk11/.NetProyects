namespace SistemaFacturacion
{
    internal class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; }

        public string ObtenerResumen()
        {
            return $"{Id}. {Nombre} | ${PrecioUnitario:0.00}";
        }
    }
}
