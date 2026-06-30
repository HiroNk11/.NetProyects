namespace RepositoryProductos
{
    internal class Producto
    {
        public Producto(int id, string nombre, decimal precio, int stock)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
        }

        public int Id { get; }
        public string Nombre { get; }
        public decimal Precio { get; }
        public int Stock { get; private set; }

        public void DescontarStock(int cantidad)
        {
            Stock -= cantidad;
        }
    }
}
