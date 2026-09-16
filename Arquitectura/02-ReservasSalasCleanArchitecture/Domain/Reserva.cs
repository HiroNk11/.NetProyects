using System;

namespace ReservasSalasCleanArchitecture.Domain
{
    internal class Reserva
    {
        public Reserva(int id, string sala, string solicitante, DateTime inicio, DateTime fin)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El id debe ser mayor a cero.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(sala))
            {
                throw new ArgumentException("La sala es obligatoria.", nameof(sala));
            }

            if (string.IsNullOrWhiteSpace(solicitante))
            {
                throw new ArgumentException("El solicitante es obligatorio.", nameof(solicitante));
            }

            if (fin <= inicio)
            {
                throw new ArgumentException("La fecha de fin debe ser posterior al inicio.", nameof(fin));
            }

            Id = id;
            Sala = sala.Trim();
            Solicitante = solicitante.Trim();
            Inicio = inicio;
            Fin = fin;
        }

        public int Id { get; }

        public string Sala { get; }

        public string Solicitante { get; }

        public DateTime Inicio { get; }

        public DateTime Fin { get; }

        public bool SeSuperponeCon(Reserva otraReserva)
        {
            return Sala.Equals(otraReserva.Sala, StringComparison.OrdinalIgnoreCase) &&
                   Inicio < otraReserva.Fin &&
                   Fin > otraReserva.Inicio;
        }
    }
}
