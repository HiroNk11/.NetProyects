using System;
using ConfiguracionAppConfig.Configuration;

namespace ConfiguracionAppConfig
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppSettingsReader settings = new AppSettingsReader();

            string appName = settings.GetString("ApplicationName", "Aplicacion");
            int maxIntentos = settings.GetInt("MaxIntentosLogin", 1);
            bool mantenimiento = settings.GetBool("ModoMantenimiento", false);
            string soporte = settings.GetString("EmailSoporte", "sin-configurar@empresa.com");

            Console.WriteLine("=== Herramientas - App.config ===");
            Console.WriteLine($"Aplicacion: {appName}");
            Console.WriteLine($"Maximo de intentos de login: {maxIntentos}");
            Console.WriteLine($"Modo mantenimiento: {(mantenimiento ? "Activo" : "Inactivo")}");
            Console.WriteLine($"Email soporte: {soporte}");

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
