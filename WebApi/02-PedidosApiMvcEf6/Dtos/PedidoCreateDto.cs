using System.Collections.Generic;

namespace PedidosApiMvcEf6.Dtos
{
    public class PedidoCreateDto
    {
        public int ClienteId { get; set; }
        public List<ItemPedidoCreateDto> Items { get; set; } = new List<ItemPedidoCreateDto>();
    }
}
