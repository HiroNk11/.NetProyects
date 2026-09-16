using System;
using AdapterExportacion.Domain;
using AdapterExportacion.Exportadores;
using AdapterExportacion.ExternalServices;
using AdapterExportacion.Services;

namespace AdapterExportacion
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ReporteVenta reporte = new ReporteVenta("Septiembre 2026", 38, 1850000);

            Mostrar("CSV", new CsvExportador(), reporte);
            Mostrar("JSON", new JsonExportadorAdapter(new JsonLibrary()), reporte);
            Mostrar("PDF", new PdfExportadorAdapter(new PdfLibrary()), reporte);

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }

        private static void Mostrar(string formato, IExportadorReporte exportador, ReporteVenta reporte)
        {
            ReporteService service = new ReporteService(exportador);
            Console.WriteLine($"=== Exportacion {formato} ===");
            Console.WriteLine(service.Generar(reporte));
            Console.WriteLine();
        }
    }
}
