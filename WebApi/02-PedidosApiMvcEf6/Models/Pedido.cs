using System;
using System.Collections.Generic;
using System.Linq;

namespace PedidosApiMvcEf6.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string Estado { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<ItemPedido> Items { get; set; } = new List<ItemPedido>();

        public decimal Total => Items.Sum(item => item.Subtotal);
    }
}
