using System.ComponentModel.DataAnnotations;

namespace ProductosApiMvcEf6.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Range(0.01, 999999999)]
        public decimal Precio { get; set; }

        [Range(0, 999999)]
        public int Stock { get; set; }

        public bool Activo { get; set; }
    }
}
