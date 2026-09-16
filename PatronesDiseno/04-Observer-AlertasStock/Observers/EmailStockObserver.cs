using System;
using ObserverAlertasStock.Domain;

namespace ObserverAlertasStock.Observers
{
    internal class EmailStockObserver : IObservadorStock
    {
        private readonly string _destinatario;

        public EmailStockObserver(string destinatario)
        {
            _destinatario = destinatario;
        }

        public void Notificar(Producto producto, string mensaje)
        {
            Console.WriteLine($"[EMAIL] Para: {_destinatario} | {mensaje} Producto: {producto.Nombre}.");
        }
    }
}
