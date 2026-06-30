using System;
using System.Collections.Generic;
using System.IO;

namespace GeneradorReportes
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ReporteVentas reporte = new ReporteVentas(new List<Venta>
            {
                new Venta("Notebook", 2, 980000),
                new Venta("Monitor", 4, 210000),
                new Venta("Teclado", 5, 76000)
            });

            Console.WriteLine("=== Generador de reportes SOLID ===");
            Console.WriteLine("1. Ver reporte de texto en consola");
            Console.WriteLine("2. Guardar reporte CSV en archivo");
            Console.Write("Seleccione una opcion: ");

            string opcion = Console.ReadLine();
            ReporteService service = CrearService(opcion);

            Console.WriteLine();
            service.Generar(reporte);

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para salir...");
            Console.ReadKey();
        }

        private static ReporteService CrearService(string opcion)
        {
            if (opcion == "2")
            {
                string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reporte_ventas.csv");
                return new ReporteService(new ReporteCsvFormatter(), new ArchivoReporteDestino(ruta));
            }

            return new ReporteService(new ReporteTextoFormatter(), new ConsolaReporteDestino());
        }
    }
}
