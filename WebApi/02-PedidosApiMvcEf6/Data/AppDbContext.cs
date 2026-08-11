using System.Data.Entity;
using PedidosApiMvcEf6.Models;

namespace PedidosApiMvcEf6.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=PedidosDb")
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemsPedido { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .HasRequired(pedido => pedido.Cliente)
                .WithMany(cliente => cliente.Pedidos)
                .HasForeignKey(pedido => pedido.ClienteId);

            modelBuilder.Entity<ItemPedido>()
                .HasRequired(item => item.Pedido)
                .WithMany(pedido => pedido.Items)
                .HasForeignKey(item => item.PedidoId);

            modelBuilder.Entity<ItemPedido>()
                .HasRequired(item => item.Producto)
                .WithMany(producto => producto.ItemsPedido)
                .HasForeignKey(item => item.ProductoId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
