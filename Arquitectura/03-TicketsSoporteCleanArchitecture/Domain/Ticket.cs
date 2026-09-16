using System;

namespace TicketsSoporteCleanArchitecture.Domain
{
    internal class Ticket
    {
        public Ticket(int id, string cliente, string descripcion, PrioridadTicket prioridad)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El id debe ser mayor a cero.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(cliente))
            {
                throw new ArgumentException("El cliente es obligatorio.", nameof(cliente));
            }

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                throw new ArgumentException("La descripcion es obligatoria.", nameof(descripcion));
            }

            Id = id;
            Cliente = cliente.Trim();
            Descripcion = descripcion.Trim();
            Prioridad = prioridad;
            Estado = EstadoTicket.Abierto;
            FechaCreacion = DateTime.Now;
        }

        public int Id { get; }

        public string Cliente { get; }

        public string Descripcion { get; }

        public PrioridadTicket Prioridad { get; }

        public EstadoTicket Estado { get; private set; }

        public string TecnicoAsignado { get; private set; }

        public DateTime FechaCreacion { get; }

        public void AsignarTecnico(string tecnico)
        {
            if (string.IsNullOrWhiteSpace(tecnico))
            {
                throw new ArgumentException("El tecnico es obligatorio.", nameof(tecnico));
            }

            TecnicoAsignado = tecnico.Trim();
            Estado = EstadoTicket.EnProceso;
        }

        public bool Resolver()
        {
            if (string.IsNullOrWhiteSpace(TecnicoAsignado))
            {
                return false;
            }

            Estado = EstadoTicket.Resuelto;
            return true;
        }

        public bool Cerrar()
        {
            if (Estado != EstadoTicket.Resuelto)
            {
                return false;
            }

            Estado = EstadoTicket.Cerrado;
            return true;
        }

        public bool EstaAbierto()
        {
            return Estado == EstadoTicket.Abierto || Estado == EstadoTicket.EnProceso;
        }
    }
}
