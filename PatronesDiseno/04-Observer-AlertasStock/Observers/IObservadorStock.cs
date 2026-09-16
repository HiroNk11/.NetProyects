using ObserverAlertasStock.Domain;

namespace ObserverAlertasStock.Observers
{
    internal interface IObservadorStock
    {
        void Notificar(Producto producto, string mensaje);
    }
}
