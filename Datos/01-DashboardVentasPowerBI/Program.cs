using System;
using System.Collections.Generic;
using System.IO;

namespace DashboardVentasPowerBI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string carpetaSalida = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            Directory.CreateDirectory(carpetaSalida);

            GeneradorDatos generador = new GeneradorDatos();
            List<Cliente> clientes = generador.CrearClientes();
            List<Producto> productos = generador.CrearProductos();
            List<Venta> ventas = generador.CrearVentas(clientes);
            List<DetalleVenta> detalles = generador.CrearDetalles(ventas, productos);

            CsvExporter.ExportarClientes(Path.Combine(carpetaSalida, "clientes.csv"), clientes);
            CsvExporter.ExportarProductos(Path.Combine(carpetaSalida, "productos.csv"), productos);
            CsvExporter.ExportarVentas(Path.Combine(carpetaSalida, "ventas.csv"), ventas);
            CsvExporter.ExportarDetalleVentas(Path.Combine(carpetaSalida, "detalle_ventas.csv"), detalles);

            Console.WriteLine("Archivos generados para Power BI:");
            Console.WriteLine(carpetaSalida);
            Console.WriteLine();
            Console.WriteLine($"Clientes: {clientes.Count}");
            Console.WriteLine($"Productos: {productos.Count}");
            Console.WriteLine($"Ventas: {ventas.Count}");
            Console.WriteLine($"Lineas de detalle: {detalles.Count}");
            Console.WriteLine();
            Console.WriteLine("Importar los CSV desde Power BI Desktop y relacionar las tablas por sus Id.");
        }
    }
}
