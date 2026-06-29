using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorProductos
{
    internal class Program
    {
        private static readonly List<Producto> Productos = new List<Producto>();
        private static int proximoId = 1;

        private static void Main(string[] args)
        {
            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarProducto();
                        break;
                    case "2":
                        ListarProductos();
                        break;
                    case "3":
                        BuscarProducto();
                        break;
                    case "4":
                        ActualizarStock();
                        break;
                    case "5":
                        EliminarProducto();
                        break;
                    case "6":
                        MostrarValorInventario();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opcion invalida. Intente nuevamente.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Programa finalizado.");
        }

        private static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Gestor de productos ===");
            Console.WriteLine("1. Agregar producto");
            Console.WriteLine("2. Listar productos");
            Console.WriteLine("3. Buscar producto por nombre");
            Console.WriteLine("4. Actualizar stock");
            Console.WriteLine("5. Eliminar producto");
            Console.WriteLine("6. Ver valor total del inventario");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void AgregarProducto()
        {
            Producto producto = new Producto
            {
                Id = proximoId,
                Nombre = PedirTexto("Ingrese el nombre: "),
                Precio = PedirDecimal("Ingrese el precio: "),
                Stock = PedirEntero("Ingrese el stock: ")
            };

            Productos.Add(producto);
            proximoId++;

            Console.WriteLine("Producto agregado correctamente.");
        }

        private static void ListarProductos()
        {
            Console.WriteLine();

            if (Productos.Count == 0)
            {
                Console.WriteLine("No hay productos cargados.");
                return;
            }

            Console.WriteLine("Listado de productos:");

            foreach (Producto producto in Productos)
            {
                Console.WriteLine(producto.ObtenerResumen());
            }
        }

        private static void BuscarProducto()
        {
            if (Productos.Count == 0)
            {
                Console.WriteLine("No hay productos cargados.");
                return;
            }

            string textoBusqueda = PedirTexto("Ingrese nombre o parte del nombre: ");
            List<Producto> resultados = Productos
                .Where(producto => producto.Nombre.IndexOf(textoBusqueda, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            Console.WriteLine();

            if (resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron productos.");
                return;
            }

            Console.WriteLine("Resultados:");

            foreach (Producto producto in resultados)
            {
                Console.WriteLine(producto.ObtenerResumen());
            }
        }

        private static void ActualizarStock()
        {
            if (Productos.Count == 0)
            {
                Console.WriteLine("No hay productos cargados.");
                return;
            }

            ListarProductos();
            int id = PedirEntero("Ingrese el ID del producto: ");
            Producto producto = BuscarProductoPorId(id);

            if (producto == null)
            {
                Console.WriteLine("No se encontro un producto con ese ID.");
                return;
            }

            int nuevoStock = PedirEntero("Ingrese el nuevo stock: ");
            producto.Stock = nuevoStock;

            Console.WriteLine("Stock actualizado correctamente.");
        }

        private static void EliminarProducto()
        {
            if (Productos.Count == 0)
            {
                Console.WriteLine("No hay productos para eliminar.");
                return;
            }

            ListarProductos();
            int id = PedirEntero("Ingrese el ID del producto a eliminar: ");
            Producto producto = BuscarProductoPorId(id);

            if (producto == null)
            {
                Console.WriteLine("No se encontro un producto con ese ID.");
                return;
            }

            Productos.Remove(producto);
            Console.WriteLine("Producto eliminado correctamente.");
        }

        private static void MostrarValorInventario()
        {
            Console.WriteLine();

            if (Productos.Count == 0)
            {
                Console.WriteLine("No hay productos cargados.");
                return;
            }

            decimal valorTotal = Productos.Sum(producto => producto.CalcularValorTotal());
            Console.WriteLine($"Valor total del inventario: ${valorTotal:0.00}");
        }

        private static Producto BuscarProductoPorId(int id)
        {
            return Productos.FirstOrDefault(producto => producto.Id == id);
        }

        private static string PedirTexto(string mensaje)
        {
            string texto;

            do
            {
                Console.Write(mensaje);
                texto = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    Console.WriteLine("El valor no puede estar vacio.");
                }
            }
            while (string.IsNullOrWhiteSpace(texto));

            return texto.Trim();
        }

        private static int PedirEntero(string mensaje)
        {
            int numero;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out numero);

                if (!esValido || numero < 0)
                {
                    Console.WriteLine("Ingrese un numero entero igual o mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return numero;
        }

        private static decimal PedirDecimal(string mensaje)
        {
            decimal numero;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = decimal.TryParse(Console.ReadLine(), out numero);

                if (!esValido || numero < 0)
                {
                    Console.WriteLine("Ingrese un importe igual o mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return numero;
        }
    }
}
