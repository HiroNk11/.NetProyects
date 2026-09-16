using InventarioCleanArchitecture.Application.Services;
using InventarioCleanArchitecture.Domain;
using InventarioCleanArchitecture.Infrastructure;
using InventarioCleanArchitecture.Presentation;

namespace InventarioCleanArchitecture
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            InMemoryProductoRepository repository = new InMemoryProductoRepository();
            CargarDatosIniciales(repository);

            InventarioService service = new InventarioService(repository);
            ConsoleMenu menu = new ConsoleMenu(service);
            menu.Ejecutar();
        }

        private static void CargarDatosIniciales(InMemoryProductoRepository repository)
        {
            repository.Agregar(new Producto(1, "Notebook empresarial", 1200000, 8, 3));
            repository.Agregar(new Producto(2, "Monitor 24 pulgadas", 260000, 12, 4));
            repository.Agregar(new Producto(3, "Teclado mecanico", 95000, 5, 5));
            repository.Agregar(new Producto(4, "Mouse inalambrico", 42000, 2, 6));
        }
    }
}
