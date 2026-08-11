using System;

namespace PedidosApiMvcEf6.Dtos
{
    public class PedidoResumenDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
    }
}
