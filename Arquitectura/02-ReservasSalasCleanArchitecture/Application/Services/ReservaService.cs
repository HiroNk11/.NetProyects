using System;
using System.Collections.Generic;
using System.Linq;
using ReservasSalasCleanArchitecture.Application.Interfaces;
using ReservasSalasCleanArchitecture.Domain;

namespace ReservasSalasCleanArchitecture.Application.Services
{
    internal class ReservaService
    {
        private readonly IReservaRepository _repository;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public bool CrearReserva(Reserva reserva, out string mensaje)
        {
            if (_repository.Existe(reserva.Id))
            {
                mensaje = "Ya existe una reserva con ese id.";
                return false;
            }

            bool haySuperposicion = _repository
                .Listar()
                .Any(reserva.SeSuperponeCon);

            if (haySuperposicion)
            {
                mensaje = "La sala ya esta reservada en ese horario.";
                return false;
            }

            _repository.Agregar(reserva);
            mensaje = "Reserva creada correctamente.";
            return true;
        }

        public IReadOnlyList<Reserva> ListarReservas()
        {
            return _repository
                .Listar()
                .OrderBy(reserva => reserva.Inicio)
                .ToList();
        }
    }
}
