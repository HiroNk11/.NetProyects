using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaFacturacion
{
    internal class Program
    {
        private static readonly List<Cliente> Clientes = new List<Cliente>();
        private static readonly List<Producto> Productos = new List<Producto>();
        private static readonly List<Factura> Facturas = new List<Factura>();
        private static int proximoClienteId = 1;
        private static int proximoProductoId = 1;
        private static int proximoNumeroFactura = 1;

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
                        AgregarCliente();
                        break;
                    case "2":
                        AgregarProducto();
                        break;
                    case "3":
                        CrearFactura();
                        break;
                    case "4":
                        ListarFacturas();
                        break;
                    case "5":
                        VerDetalleFactura();
                        break;
                    case "6":
                        MostrarTotalFacturado();
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
            Console.WriteLine("=== Sistema de facturacion ===");
            Console.WriteLine("1. Agregar cliente");
            Console.WriteLine("2. Agregar producto");
            Console.WriteLine("3. Crear factura");
            Console.WriteLine("4. Listar facturas");
            Console.WriteLine("5. Ver detalle de factura");
            Console.WriteLine("6. Ver total facturado");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void AgregarCliente()
        {
            Cliente cliente = new Cliente
            {
                Id = proximoClienteId,
                RazonSocial = PedirTexto("Ingrese la razon social: "),
                Cuit = PedirTexto("Ingrese el CUIT: ")
            };

            Clientes.Add(cliente);
            proximoClienteId++;

            Console.WriteLine("Cliente agregado correctamente.");
        }

        private static void AgregarProducto()
        {
            Producto producto = new Producto
            {
                Id = proximoProductoId,
                Nombre = PedirTexto("Ingrese el nombre del producto: "),
                PrecioUnitario = PedirImporte("Ingrese el precio unitario: ")
            };

            Productos.Add(producto);
            proximoProductoId++;

            Console.WriteLine("Producto agregado correctamente.");
        }

        private static void CrearFactura()
        {
            if (Clientes.Count == 0 || Productos.Count == 0)
            {
                Console.WriteLine("Debe cargar al menos un cliente y un producto.");
                return;
            }

            Cliente cliente = SeleccionarCliente();

            if (cliente == null)
            {
                return;
            }

            Factura factura = new Factura
            {
                Numero = proximoNumeroFactura,
                Fecha = DateTime.Now,
                Cliente = cliente
            };

            bool agregarOtroItem;

            do
            {
                Producto producto = SeleccionarProducto();

                if (producto == null)
                {
                    return;
                }

                int cantidad = PedirEntero("Ingrese la cantidad: ");
                factura.Items.Add(new ItemFactura { Producto = producto, Cantidad = cantidad });

                agregarOtroItem = PreguntarSiNo("Desea agregar otro item? (s/n): ");
            }
            while (agregarOtroItem);

            Facturas.Add(factura);
            proximoNumeroFactura++;

            Console.WriteLine("Factura creada correctamente.");
            Console.WriteLine(factura.ObtenerResumen());
        }

        private static Cliente SeleccionarCliente()
        {
            Console.WriteLine();
            Console.WriteLine("Clientes:");

            foreach (Cliente cliente in Clientes)
            {
                Console.WriteLine(cliente.ObtenerResumen());
            }

            int id = PedirEntero("Ingrese el ID del cliente: ");
            Cliente seleccionado = Clientes.FirstOrDefault(cliente => cliente.Id == id);

            if (seleccionado == null)
            {
                Console.WriteLine("No se encontro un cliente con ese ID.");
            }

            return seleccionado;
        }

        private static Producto SeleccionarProducto()
        {
            Console.WriteLine();
            Console.WriteLine("Productos:");

            foreach (Producto producto in Productos)
            {
                Console.WriteLine(producto.ObtenerResumen());
            }

            int id = PedirEntero("Ingrese el ID del producto: ");
            Producto seleccionado = Productos.FirstOrDefault(producto => producto.Id == id);

            if (seleccionado == null)
            {
                Console.WriteLine("No se encontro un producto con ese ID.");
            }

            return seleccionado;
        }

        private static void ListarFacturas()
        {
            Console.WriteLine();

            if (Facturas.Count == 0)
            {
                Console.WriteLine("No hay facturas registradas.");
                return;
            }

            foreach (Factura factura in Facturas)
            {
                Console.WriteLine(factura.ObtenerResumen());
            }
        }

        private static void VerDetalleFactura()
        {
            if (Facturas.Count == 0)
            {
                Console.WriteLine("No hay facturas registradas.");
                return;
            }

            ListarFacturas();
            int numero = PedirEntero("Ingrese el numero de factura: ");
            Factura factura = Facturas.FirstOrDefault(item => item.Numero == numero);

            if (factura == null)
            {
                Console.WriteLine("No se encontro una factura con ese numero.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(factura.ObtenerResumen());
            Console.WriteLine($"Cliente: {factura.Cliente.RazonSocial} - CUIT {factura.Cliente.Cuit}");
            Console.WriteLine("Items:");

            foreach (ItemFactura item in factura.Items)
            {
                Console.WriteLine(item.ObtenerResumen());
            }

            Console.WriteLine($"Subtotal: ${factura.CalcularSubtotal():0.00}");
            Console.WriteLine($"IVA 21%: ${factura.CalcularIva():0.00}");
            Console.WriteLine($"Total: ${factura.CalcularTotal():0.00}");
        }

        private static void MostrarTotalFacturado()
        {
            Console.WriteLine();

            if (Facturas.Count == 0)
            {
                Console.WriteLine("No hay facturas registradas.");
                return;
            }

            decimal total = Facturas.Sum(factura => factura.CalcularTotal());
            Console.WriteLine($"Total facturado: ${total:0.00}");
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
