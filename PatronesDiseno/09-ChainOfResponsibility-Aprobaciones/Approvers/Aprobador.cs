using System;
using ChainOfResponsibilityAprobaciones.Domain;

namespace ChainOfResponsibilityAprobaciones.Approvers
{
    internal abstract class Aprobador
    {
        private Aprobador _siguiente;

        protected Aprobador(string nombre, decimal limite)
        {
            Nombre = nombre;
            Limite = limite;
        }

        protected string Nombre { get; }

        protected decimal Limite { get; }

        public Aprobador DefinirSiguiente(Aprobador siguiente)
        {
            _siguiente = siguiente;
            return siguiente;
        }

        public void Procesar(SolicitudGasto solicitud)
        {
            if (solicitud.Importe <= Limite)
            {
                Console.WriteLine($"{Nombre} aprobo ${solicitud.Importe:0.00} para {solicitud.Concepto}.");
                return;
            }

            if (_siguiente != null)
            {
                _siguiente.Procesar(solicitud);
                return;
            }

            Console.WriteLine($"Solicitud rechazada: ${solicitud.Importe:0.00} supera todos los limites.");
        }
    }
}
