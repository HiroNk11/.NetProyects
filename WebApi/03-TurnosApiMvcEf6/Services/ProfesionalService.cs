using System.Collections.Generic;
using TurnosApiMvcEf6.Dtos;
using TurnosApiMvcEf6.Models;
using TurnosApiMvcEf6.Repositories;

namespace TurnosApiMvcEf6.Services
{
    public class ProfesionalService
    {
        private readonly IProfesionalRepository repository;

        public ProfesionalService(IProfesionalRepository repository)
        {
            this.repository = repository;
        }

        public List<Profesional> ObtenerTodos()
        {
            return repository.ObtenerTodos();
        }

        public string Crear(ProfesionalCreateDto dto, out Profesional profesional)
        {
            profesional = null;

            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(dto.Especialidad))
            {
                return "La especialidad es obligatoria.";
            }

            profesional = new Profesional { Nombre = dto.Nombre.Trim(), Especialidad = dto.Especialidad.Trim() };
            repository.Agregar(profesional);
            return string.Empty;
        }
    }
}
