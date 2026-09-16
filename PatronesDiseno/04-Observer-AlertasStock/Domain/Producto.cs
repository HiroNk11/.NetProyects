using System.Collections.Generic;
using ObserverAlertasStock.Observers;

namespace ObserverAlertasStock.Domain
{
    internal class Producto
    {
        private readonly List<IObservadorStock> _observadores = new List<IObservadorStock>();

        public Producto(int id, string nombre, int stock, int stockMinimo)
        {
            Id = id;
            Nombre = nombre;
            Stock = stock;
            StockMinimo = stockMinimo;
        }

        public int Id { get; }

        public string Nombre { get; }

        public int Stock { get; private set; }

        public int StockMinimo { get; }

        public void Suscribir(IObservadorStock observador)
        {
            if (!_observadores.Contains(observador))
            {
                _observadores.Add(observador);
            }
        }

        public void Desuscribir(IObservadorStock observador)
        {
            _observadores.Remove(observador);
        }

        public bool DescontarStock(int cantidad)
        {
            if (cantidad <= 0 || cantidad > Stock)
            {
                return false;
            }

            Stock -= cantidad;
            EvaluarStock();
            return true;
        }

        public void ReponerStock(int cantidad)
        {
            if (cantidad <= 0)
            {
                return;
            }

            Stock += cantidad;
            EvaluarStock();
        }

        private void EvaluarStock()
        {
            if (Stock > StockMinimo)
            {
                return;
            }

            foreach (IObservadorStock observador in _observadores)
            {
                observador.Notificar(this, "El producto llego al stock minimo.");
            }
        }
    }
}
