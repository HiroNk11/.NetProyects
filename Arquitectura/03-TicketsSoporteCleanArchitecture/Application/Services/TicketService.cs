using System;
using System.Collections.Generic;
using System.Linq;
using TicketsSoporteCleanArchitecture.Application.Interfaces;
using TicketsSoporteCleanArchitecture.Domain;

namespace TicketsSoporteCleanArchitecture.Application.Services
{
    internal class TicketService
    {
        private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public bool CrearTicket(Ticket ticket, out string mensaje)
        {
            if (_repository.Existe(ticket.Id))
            {
                mensaje = "Ya existe un ticket con ese id.";
                return false;
            }

            _repository.Agregar(ticket);
            mensaje = "Ticket creado correctamente.";
            return true;
        }

        public bool AsignarTecnico(int id, string tecnico, out string mensaje)
        {
            Ticket ticket = _repository.BuscarPorId(id);

            if (ticket == null)
            {
                mensaje = "No se encontro el ticket.";
                return false;
            }

            ticket.AsignarTecnico(tecnico);
            mensaje = "Tecnico asignado correctamente.";
            return true;
        }

        public bool ResolverTicket(int id, out string mensaje)
        {
            Ticket ticket = _repository.BuscarPorId(id);

            if (ticket == null)
            {
                mensaje = "No se encontro el ticket.";
                return false;
            }

            bool resuelto = ticket.Resolver();
            mensaje = resuelto ? "Ticket resuelto." : "Debe asignar un tecnico antes de resolver.";
            return resuelto;
        }

        public IReadOnlyList<Ticket> ListarAbiertos()
        {
            return _repository
                .Listar()
                .Where(ticket => ticket.EstaAbierto())
                .OrderByDescending(ticket => ticket.Prioridad)
                .ToList();
        }

        public IReadOnlyList<Ticket> ListarPorPrioridad(PrioridadTicket prioridad)
        {
            return _repository
                .Listar()
                .Where(ticket => ticket.Prioridad == prioridad)
                .OrderBy(ticket => ticket.FechaCreacion)
                .ToList();
        }
    }
}
