using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PedidosApiMvcEf6.Data;
using PedidosApiMvcEf6.Models;

namespace PedidosApiMvcEf6.Repositories
{
    public class EfProductoRepository : IProductoRepository
    {
        private readonly AppDbContext context;

        public EfProductoRepository(AppDbContext context)
        {
            this.context = context;
        }

        public List<Producto> ObtenerTodos()
        {
            return context.Productos.OrderBy(producto => producto.Nombre).ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            return context.Productos.Find(id);
        }

        public void Agregar(Producto producto)
        {
            context.Productos.Add(producto);
            context.SaveChanges();
        }

        public void Actualizar(Producto producto)
        {
            context.Entry(producto).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
