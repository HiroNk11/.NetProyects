using System;
using System.Collections.Generic;
using System.IO;
using LoggerArchivos.Logging;
using LoggerArchivos.Services;

namespace LoggerArchivos
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app.log");

            ILogger logger = new CompositeLogger(new List<ILogger>
            {
                new ConsoleLogger(),
                new FileLogger(logPath)
            });

            ImportacionService service = new ImportacionService(logger);

            Console.WriteLine("=== Herramientas - Logger archivos ===");
            service.Ejecutar();

            Console.WriteLine();
            Console.WriteLine($"Log generado en: {logPath}");
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
