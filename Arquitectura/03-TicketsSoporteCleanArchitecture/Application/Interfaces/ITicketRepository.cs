using System.Collections.Generic;
using TicketsSoporteCleanArchitecture.Domain;

namespace TicketsSoporteCleanArchitecture.Application.Interfaces
{
    internal interface ITicketRepository
    {
        void Agregar(Ticket ticket);

        Ticket BuscarPorId(int id);

        IReadOnlyList<Ticket> Listar();

        bool Existe(int id);
    }
}
