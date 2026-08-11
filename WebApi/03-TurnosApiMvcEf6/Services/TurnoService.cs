using System;
using System.Collections.Generic;
using System.Linq;
using TurnosApiMvcEf6.Dtos;
using TurnosApiMvcEf6.Models;
using TurnosApiMvcEf6.Repositories;

namespace TurnosApiMvcEf6.Services
{
    public class TurnoService
    {
        private const string EstadoActivo = "Activo";
        private const string EstadoCancelado = "Cancelado";

        private readonly IPacienteRepository pacienteRepository;
        private readonly IProfesionalRepository profesionalRepository;
        private readonly ITurnoRepository turnoRepository;

        public TurnoService(IPacienteRepository pacienteRepository, IProfesionalRepository profesionalRepository, ITurnoRepository turnoRepository)
        {
            this.pacienteRepository = pacienteRepository;
            this.profesionalRepository = profesionalRepository;
            this.turnoRepository = turnoRepository;
        }

        public List<TurnoResumenDto> ObtenerTodos()
        {
            return turnoRepository.ObtenerTodos().Select(MapearResumen).ToList();
        }

        public Turno ObtenerPorId(int id)
        {
            return turnoRepository.ObtenerPorId(id);
        }

        public string Crear(TurnoCreateDto dto, out Turno turno)
        {
            turno = null;
            Paciente paciente = pacienteRepository.ObtenerPorId(dto.PacienteId);
            Profesional profesional = profesionalRepository.ObtenerPorId(dto.ProfesionalId);

            if (paciente == null)
            {
                return "Paciente no encontrado.";
            }

            if (profesional == null)
            {
                return "Profesional no encontrado.";
            }

            if (dto.FechaHora <= DateTime.Now)
            {
                return "La fecha del turno debe ser futura.";
            }

            if (dto.DuracionMinutos <= 0 || dto.DuracionMinutos > 240)
            {
                return "La duracion debe estar entre 1 y 240 minutos.";
            }

            DateTime nuevoFin = dto.FechaHora.AddMinutes(dto.DuracionMinutos);
            bool haySuperposicion = turnoRepository.ObtenerPorProfesional(dto.ProfesionalId)
                .Any(existente =>
                    existente.Estado != EstadoCancelado &&
                    dto.FechaHora < existente.FechaFin &&
                    nuevoFin > existente.FechaHora);

            if (haySuperposicion)
            {
                return "El profesional ya tiene un turno en ese horario.";
            }

            turno = new Turno
            {
                PacienteId = paciente.Id,
                ProfesionalId = profesional.Id,
                Paciente = paciente,
                Profesional = profesional,
                FechaHora = dto.FechaHora,
                DuracionMinutos = dto.DuracionMinutos,
                Motivo = dto.Motivo,
                Estado = EstadoActivo
            };

            turnoRepository.Agregar(turno);
            return string.Empty;
        }

        public bool Cancelar(int id)
        {
            Turno turno = turnoRepository.ObtenerPorId(id);

            if (turno == null || turno.Estado == EstadoCancelado)
            {
                return false;
            }

            turno.Estado = EstadoCancelado;
            turnoRepository.Actualizar(turno);
            return true;
        }

        private TurnoResumenDto MapearResumen(Turno turno)
        {
            return new TurnoResumenDto
            {
                Id = turno.Id,
                FechaHora = turno.FechaHora,
                DuracionMinutos = turno.DuracionMinutos,
                Paciente = turno.Paciente.Nombre,
                Profesional = turno.Profesional.Nombre,
                Especialidad = turno.Profesional.Especialidad,
                Estado = turno.Estado,
                Motivo = turno.Motivo
            };
        }
    }
}
