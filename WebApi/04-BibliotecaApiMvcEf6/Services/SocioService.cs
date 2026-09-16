using System.Collections.Generic;
using BibliotecaApiMvcEf6.Dtos;
using BibliotecaApiMvcEf6.Models;
using BibliotecaApiMvcEf6.Repositories;

namespace BibliotecaApiMvcEf6.Services
{
    public class SocioService
    {
        private readonly ISocioRepository repository;

        public SocioService(ISocioRepository repository)
        {
            this.repository = repository;
        }

        public List<Socio> ObtenerTodos()
        {
            return repository.ObtenerTodos();
        }

        public string Crear(SocioCreateDto dto, out Socio socio)
        {
            socio = null;

            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
            {
                return "Ingrese un email valido.";
            }

            socio = new Socio { Nombre = dto.Nombre.Trim(), Email = dto.Email.Trim(), Activo = true };
            repository.Agregar(socio);
            return string.Empty;
        }
    }
}
