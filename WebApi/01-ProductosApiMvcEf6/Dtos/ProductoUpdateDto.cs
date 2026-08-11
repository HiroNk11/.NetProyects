namespace ProductosApiMvcEf6.Dtos
{
    public class ProductoUpdateDto
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
    }
}
