using System.Collections.Generic;
using System.Linq;
using ReservasSalasCleanArchitecture.Application.Interfaces;
using ReservasSalasCleanArchitecture.Domain;

namespace ReservasSalasCleanArchitecture.Infrastructure
{
    internal class InMemoryReservaRepository : IReservaRepository
    {
        private readonly List<Reserva> _reservas = new List<Reserva>();

        public void Agregar(Reserva reserva)
        {
            _reservas.Add(reserva);
        }

        public IReadOnlyList<Reserva> Listar()
        {
            return _reservas.ToList();
        }

        public bool Existe(int id)
        {
            return _reservas.Any(reserva => reserva.Id == id);
        }
    }
}
