using System.Collections.Generic;
using TurnosApiMvcEf6.Dtos;
using TurnosApiMvcEf6.Models;
using TurnosApiMvcEf6.Repositories;

namespace TurnosApiMvcEf6.Services
{
    public class PacienteService
    {
        private readonly IPacienteRepository repository;

        public PacienteService(IPacienteRepository repository)
        {
            this.repository = repository;
        }

        public List<Paciente> ObtenerTodos()
        {
            return repository.ObtenerTodos();
        }

        public string Crear(PacienteCreateDto dto, out Paciente paciente)
        {
            paciente = null;

            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(dto.Documento))
            {
                return "El documento es obligatorio.";
            }

            paciente = new Paciente { Nombre = dto.Nombre.Trim(), Documento = dto.Documento.Trim() };
            repository.Agregar(paciente);
            return string.Empty;
        }
    }
}
