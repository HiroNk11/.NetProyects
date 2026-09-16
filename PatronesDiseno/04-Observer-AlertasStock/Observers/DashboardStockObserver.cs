using System;
using ObserverAlertasStock.Domain;

namespace ObserverAlertasStock.Observers
{
    internal class DashboardStockObserver : IObservadorStock
    {
        public void Notificar(Producto producto, string mensaje)
        {
            Console.WriteLine($"[DASHBOARD] Actualizando tarjeta de stock bajo para {producto.Nombre}.");
        }
    }
}
