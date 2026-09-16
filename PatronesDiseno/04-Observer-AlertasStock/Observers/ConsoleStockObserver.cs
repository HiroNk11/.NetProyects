using System;
using ObserverAlertasStock.Domain;

namespace ObserverAlertasStock.Observers
{
    internal class ConsoleStockObserver : IObservadorStock
    {
        public void Notificar(Producto producto, string mensaje)
        {
            Console.WriteLine($"[CONSOLA] {mensaje} Producto: {producto.Nombre}. Stock actual: {producto.Stock}.");
        }
    }
}
