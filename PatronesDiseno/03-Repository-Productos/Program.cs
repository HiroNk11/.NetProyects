using System;
using System.Collections.Generic;

namespace RepositoryProductos
{
    internal class Program
    {
        private static ProductoService _service;

        private static void Main(string[] args)
        {
            IProductoRepository repository = new ProductoRepositoryMemoria();
            CargarProductos(repository);
            _service = new ProductoService(repository);

            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=== Repository - Productos ===");
                Console.WriteLine("1. Listar productos");
                Console.WriteLine("2. Registrar venta");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opcion: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Listar();
                        break;
                    case "2":
                        Vender();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opcion invalida.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private static void CargarProductos(IProductoRepository repository)
        {
            repository.Agregar(new Producto(1, "Notebook", 980000, 5));
            repository.Agregar(new Producto(2, "Monitor", 210000, 12));
            repository.Agregar(new Producto(3, "Teclado", 76000, 20));
            repository.Agregar(new Producto(4, "Mouse", 35000, 25));
        }

        private static void Listar()
        {
            List<Producto> productos = _service.Listar();

            Console.WriteLine();
            Console.WriteLine("Productos:");

            foreach (Producto producto in productos)
            {
                Console.WriteLine($"{producto.Id}. {producto.Nombre} | Precio: ${producto.Precio:0.00} | Stock: {producto.Stock}");
            }
        }

        private static void Vender()
        {
            int id = PedirEntero("Id del producto: ");
            int cantidad = PedirEntero("Cantidad: ");

            bool ventaExitosa = _service.Vender(id, cantidad);
            Console.WriteLine(ventaExitosa ? "Venta registrada correctamente." : "No se pudo registrar la venta.");
        }

        private static int PedirEntero(string mensaje)
        {
            int valor;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out valor);

                if (!esValido || valor <= 0)
                {
                    Console.WriteLine("Ingrese un numero mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return valor;
        }
    }
}
