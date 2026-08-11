using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PedidosApiMvcEf6.Data;
using PedidosApiMvcEf6.Models;

namespace PedidosApiMvcEf6.Repositories
{
    public class EfPedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext context;

        public EfPedidoRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Pedido> ObtenerTodos()
        {
            return context.Pedidos
                .Include(pedido => pedido.Cliente)
                .Include(pedido => pedido.Items.Select(item => item.Producto))
                .OrderByDescending(pedido => pedido.Fecha)
                .ToList();
        }

        public Pedido ObtenerPorId(int id)
        {
            return context.Pedidos
                .Include(pedido => pedido.Cliente)
                .Include(pedido => pedido.Items.Select(item => item.Producto))
                .FirstOrDefault(pedido => pedido.Id == id);
        }

        public void Agregar(Pedido pedido)
        {
            context.Pedidos.Add(pedido);
            context.SaveChanges();
        }

        public void Actualizar(Pedido pedido)
        {
            context.Entry(pedido).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
