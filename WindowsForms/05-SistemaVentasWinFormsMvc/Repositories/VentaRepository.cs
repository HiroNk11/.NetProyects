using System.Collections.Generic;
using System.Linq;
using SistemaVentasWinFormsMvc.Models;

namespace SistemaVentasWinFormsMvc.Repositories
{
    internal class VentaRepository : IVentaRepository
    {
        private readonly MemoryStore store;

        public VentaRepository(MemoryStore store)
        {
            this.store = store;
        }

        public List<Venta> ObtenerTodas()
        {
            return store.Ventas.ToList();
        }

        public void Agregar(Venta venta)
        {
            venta.Id = store.ProximaVentaId;
            store.ProximaVentaId++;
            store.Ventas.Add(venta);
        }
    }
}
