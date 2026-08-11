using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ProductosApiMvcEf6.Data;
using ProductosApiMvcEf6.Models;

namespace ProductosApiMvcEf6.Repositories
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
            return context.Productos
                .OrderBy(producto => producto.Nombre)
                .ToList();
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

        public void Eliminar(Producto producto)
        {
            context.Productos.Remove(producto);
            context.SaveChanges();
        }
    }
}
