using System.Collections.Generic;
using System.Linq;
using TicketsSoporteCleanArchitecture.Application.Interfaces;
using TicketsSoporteCleanArchitecture.Domain;

namespace TicketsSoporteCleanArchitecture.Infrastructure
{
    internal class InMemoryTicketRepository : ITicketRepository
    {
        private readonly List<Ticket> _tickets = new List<Ticket>();

        public void Agregar(Ticket ticket)
        {
            _tickets.Add(ticket);
        }

        public Ticket BuscarPorId(int id)
        {
            return _tickets.FirstOrDefault(ticket => ticket.Id == id);
        }

        public IReadOnlyList<Ticket> Listar()
        {
            return _tickets.ToList();
        }

        public bool Existe(int id)
        {
            return _tickets.Any(ticket => ticket.Id == id);
        }
    }
}
