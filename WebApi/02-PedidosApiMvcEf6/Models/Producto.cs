using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PedidosApiMvcEf6.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }

        public virtual ICollection<ItemPedido> ItemsPedido { get; set; } = new List<ItemPedido>();
    }
}
