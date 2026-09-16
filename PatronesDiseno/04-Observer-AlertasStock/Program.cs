using System;
using ObserverAlertasStock.Domain;
using ObserverAlertasStock.Observers;

namespace ObserverAlertasStock
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Producto monitor = new Producto(1, "Monitor 24 pulgadas", 6, 3);

            monitor.Suscribir(new ConsoleStockObserver());
            monitor.Suscribir(new EmailStockObserver("compras@empresa.com"));
            monitor.Suscribir(new DashboardStockObserver());

            Console.WriteLine("=== Observer - Alertas de stock ===");
            MostrarStock(monitor);

            Console.WriteLine();
            Console.WriteLine("Venta de 2 unidades.");
            monitor.DescontarStock(2);
            MostrarStock(monitor);

            Console.WriteLine();
            Console.WriteLine("Venta de 1 unidad.");
            monitor.DescontarStock(1);
            MostrarStock(monitor);

            Console.WriteLine();
            Console.WriteLine("Reposicion de 5 unidades.");
            monitor.ReponerStock(5);
            MostrarStock(monitor);

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }

        private static void MostrarStock(Producto producto)
        {
            Console.WriteLine($"{producto.Nombre} | Stock actual: {producto.Stock} | Minimo: {producto.StockMinimo}");
        }
    }
}
