using System;
using System.Collections.Generic;
using System.Linq;

namespace RegistroVentas
{
    internal class Program
    {
        private static readonly List<Venta> Ventas = new List<Venta>();
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
                        RegistrarVenta();
                        break;
                    case "2":
                        ListarVentas();
                        break;
                    case "3":
                        VerDetalleVenta();
                        break;
                    case "4":
                        MostrarTotalVendido();
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
            Console.WriteLine("=== Registro de ventas ===");
            Console.WriteLine("1. Registrar venta");
            Console.WriteLine("2. Listar ventas");
            Console.WriteLine("3. Ver detalle de una venta");
            Console.WriteLine("4. Ver total vendido");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void RegistrarVenta()
        {
            Venta venta = new Venta
            {
                Id = proximoId,
                Fecha = DateTime.Now,
                Cliente = PedirTexto("Ingrese el nombre del cliente: ")
            };

            bool agregarOtroProducto;

            do
            {
                ProductoVenta producto = new ProductoVenta
                {
                    Nombre = PedirTexto("Ingrese el producto: "),
                    Cantidad = PedirEntero("Ingrese la cantidad: "),
                    PrecioUnitario = PedirImporte("Ingrese el precio unitario: ")
                };

                venta.Productos.Add(producto);
                agregarOtroProducto = PreguntarSiNo("Desea agregar otro producto? (s/n): ");
            }
            while (agregarOtroProducto);

            Ventas.Add(venta);
            proximoId++;

            Console.WriteLine($"Venta registrada correctamente. Total: ${venta.CalcularTotal():0.00}");
        }

        private static void ListarVentas()
        {
            Console.WriteLine();

            if (Ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }

            Console.WriteLine("Listado de ventas:");

            foreach (Venta venta in Ventas)
            {
                Console.WriteLine(venta.ObtenerResumen());
            }
        }

        private static void VerDetalleVenta()
        {
            if (Ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }

            ListarVentas();
            int id = PedirEntero("Ingrese el ID de la venta: ");
            Venta venta = BuscarVentaPorId(id);

            if (venta == null)
            {
                Console.WriteLine("No se encontro una venta con ese ID.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(venta.ObtenerResumen());
            Console.WriteLine("Detalle:");

            foreach (ProductoVenta producto in venta.Productos)
            {
                Console.WriteLine(producto.ObtenerResumen());
            }
        }

        private static void MostrarTotalVendido()
        {
            Console.WriteLine();

            if (Ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }

            decimal totalVendido = Ventas.Sum(venta => venta.CalcularTotal());
            Console.WriteLine($"Total vendido: ${totalVendido:0.00}");
        }

        private static Venta BuscarVentaPorId(int id)
        {
            return Ventas.FirstOrDefault(venta => venta.Id == id);
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

                if (!esValido || numero <= 0)
                {
                    Console.WriteLine("Ingrese un numero entero mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return numero;
        }

        private static decimal PedirImporte(string mensaje)
        {
            decimal importe;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = decimal.TryParse(Console.ReadLine(), out importe);

                if (!esValido || importe <= 0)
                {
                    Console.WriteLine("Ingrese un importe mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return importe;
        }

        private static bool PreguntarSiNo(string mensaje)
        {
            string respuesta;

            do
            {
                Console.Write(mensaje);
                respuesta = Console.ReadLine().ToLower();

                if (respuesta == "s")
                {
                    return true;
                }

                if (respuesta == "n")
                {
                    return false;
                }

                Console.WriteLine("Respuesta invalida. Ingrese 's' o 'n'.");
            }
            while (true);
        }
    }
}
