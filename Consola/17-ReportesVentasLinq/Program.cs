using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportesVentasLinq
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Venta> ventas = CrearVentas();

            Console.WriteLine("=== Reportes de ventas con LINQ ===");
            MostrarTotalPorVendedor(ventas);
            MostrarProductoMasVendido(ventas);
            MostrarVentasAltas(ventas, 200000);
            MostrarPromedioPorRegion(ventas);

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }

        private static void MostrarTotalPorVendedor(IEnumerable<Venta> ventas)
        {
            Console.WriteLine();
            Console.WriteLine("Total facturado por vendedor:");

            var consulta = ventas
                .GroupBy(venta => venta.Vendedor)
                .Select(grupo => new { Vendedor = grupo.Key, Total = grupo.Sum(venta => venta.Total) })
                .OrderByDescending(resultado => resultado.Total);

            foreach (var item in consulta)
            {
                Console.WriteLine($"{item.Vendedor}: ${item.Total:0.00}");
            }
        }

        private static void MostrarProductoMasVendido(IEnumerable<Venta> ventas)
        {
            var producto = ventas
                .GroupBy(venta => venta.Producto)
                .Select(grupo => new { Producto = grupo.Key, Cantidad = grupo.Sum(venta => venta.Cantidad) })
                .OrderByDescending(resultado => resultado.Cantidad)
                .First();

            Console.WriteLine();
            Console.WriteLine($"Producto mas vendido: {producto.Producto} ({producto.Cantidad} unidades)");
        }

        private static void MostrarVentasAltas(IEnumerable<Venta> ventas, decimal importeMinimo)
        {
            Console.WriteLine();
            Console.WriteLine($"Ventas mayores a ${importeMinimo:0.00}:");

            foreach (Venta venta in ventas.Where(venta => venta.Total >= importeMinimo))
            {
                Console.WriteLine($"{venta.Fecha:yyyy-MM-dd} | {venta.Vendedor} | {venta.Producto} | ${venta.Total:0.00}");
            }
        }

        private static void MostrarPromedioPorRegion(IEnumerable<Venta> ventas)
        {
            Console.WriteLine();
            Console.WriteLine("Promedio por region:");

            var consulta = ventas
                .GroupBy(venta => venta.Region)
                .Select(grupo => new { Region = grupo.Key, Promedio = grupo.Average(venta => venta.Total) });

            foreach (var item in consulta)
            {
                Console.WriteLine($"{item.Region}: ${item.Promedio:0.00}");
            }
        }

        private static List<Venta> CrearVentas()
        {
            return new List<Venta>
            {
                new Venta(DateTime.Today.AddDays(-5), "Ana", "Norte", "Notebook", 1, 1200000),
                new Venta(DateTime.Today.AddDays(-4), "Bruno", "Sur", "Monitor", 2, 260000),
                new Venta(DateTime.Today.AddDays(-3), "Ana", "Norte", "Teclado", 4, 95000),
                new Venta(DateTime.Today.AddDays(-2), "Carla", "Centro", "Mouse", 5, 42000),
                new Venta(DateTime.Today.AddDays(-1), "Bruno", "Sur", "Notebook", 1, 1180000),
                new Venta(DateTime.Today, "Carla", "Centro", "Monitor", 1, 255000)
            };
        }
    }
}
