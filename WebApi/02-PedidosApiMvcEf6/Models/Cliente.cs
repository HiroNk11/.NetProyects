using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PedidosApiMvcEf6.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(120)]
        public string Email { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
