using System;
using System.Collections.Generic;
using InventarioCleanArchitecture.Application.Services;
using InventarioCleanArchitecture.Domain;

namespace InventarioCleanArchitecture.Presentation
{
    internal class ConsoleMenu
    {
        private readonly InventarioService _inventarioService;

        public ConsoleMenu(InventarioService inventarioService)
        {
            _inventarioService = inventarioService;
        }

        public void Ejecutar()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=== Inventario Clean Architecture ===");
                Console.WriteLine("1. Listar productos");
                Console.WriteLine("2. Agregar producto");
                Console.WriteLine("3. Registrar entrada de stock");
                Console.WriteLine("4. Registrar salida de stock");
                Console.WriteLine("5. Ver stock bajo");
                Console.WriteLine("6. Ver valor total del inventario");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opcion: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ListarProductos(_inventarioService.ListarProductos());
                        break;
                    case "2":
                        AgregarProducto();
                        break;
                    case "3":
                        RegistrarMovimiento(esEntrada: true);
                        break;
                    case "4":
                        RegistrarMovimiento(esEntrada: false);
                        break;
                    case "5":
                        ListarProductos(_inventarioService.ObtenerProductosConStockBajo());
                        break;
                    case "6":
                        Console.WriteLine($"Valor total: ${_inventarioService.CalcularValorTotal():0.00}");
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

        private void AgregarProducto()
        {
            try
            {
                int id = PedirEntero("Id: ");
                Console.Write("Nombre: ");
                string nombre = Console.ReadLine();
                decimal precio = PedirDecimal("Precio: ");
                int stock = PedirEntero("Stock inicial: ", permiteCero: true);
                int stockMinimo = PedirEntero("Stock minimo: ", permiteCero: true);

                _inventarioService.AgregarProducto(new Producto(id, nombre, precio, stock, stockMinimo));
                Console.WriteLine("Producto agregado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo agregar el producto: {ex.Message}");
            }
        }

        private void RegistrarMovimiento(bool esEntrada)
        {
            try
            {
                int id = PedirEntero("Id del producto: ");
                int cantidad = PedirEntero("Cantidad: ");

                bool resultado = esEntrada
                    ? _inventarioService.RegistrarEntrada(id, cantidad)
                    : _inventarioService.RegistrarSalida(id, cantidad);

                Console.WriteLine(resultado ? "Movimiento registrado." : "No se pudo registrar el movimiento.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Operacion invalida: {ex.Message}");
            }
        }

        private static void ListarProductos(IReadOnlyList<Producto> productos)
        {
            if (productos.Count == 0)
            {
                Console.WriteLine("No hay productos para mostrar.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Productos:");

            foreach (Producto producto in productos)
            {
                Console.WriteLine(
                    $"{producto.Id}. {producto.Nombre} | Precio: ${producto.Precio:0.00} | Stock: {producto.Stock} | Minimo: {producto.StockMinimo}");
            }
        }

        private static int PedirEntero(string mensaje, bool permiteCero = false)
        {
            int valor;

            while (true)
            {
                Console.Write(mensaje);

                if (int.TryParse(Console.ReadLine(), out valor) && (valor > 0 || permiteCero && valor == 0))
                {
                    return valor;
                }

                Console.WriteLine(permiteCero ? "Ingrese cero o un numero mayor." : "Ingrese un numero mayor a cero.");
            }
        }

        private static decimal PedirDecimal(string mensaje)
        {
            decimal valor;

            while (true)
            {
                Console.Write(mensaje);

                if (decimal.TryParse(Console.ReadLine(), out valor) && valor > 0)
                {
                    return valor;
                }

                Console.WriteLine("Ingrese un importe mayor a cero.");
            }
        }
    }
}
