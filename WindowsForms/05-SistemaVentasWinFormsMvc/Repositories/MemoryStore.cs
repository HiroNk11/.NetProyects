using System.Collections.Generic;
using SistemaVentasWinFormsMvc.Models;

namespace SistemaVentasWinFormsMvc.Repositories
{
    internal class MemoryStore
    {
        public List<Cliente> Clientes { get; } = new List<Cliente>();
        public List<Producto> Productos { get; } = new List<Producto>();
        public List<Venta> Ventas { get; } = new List<Venta>();
        public int ProximoClienteId { get; set; } = 1;
        public int ProximoProductoId { get; set; } = 1;
        public int ProximaVentaId { get; set; } = 1;
    }
}
