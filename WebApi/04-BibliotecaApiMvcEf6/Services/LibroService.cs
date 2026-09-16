using System;
using System.Collections.Generic;
using BibliotecaApiMvcEf6.Dtos;
using BibliotecaApiMvcEf6.Models;
using BibliotecaApiMvcEf6.Repositories;

namespace BibliotecaApiMvcEf6.Services
{
    public class LibroService
    {
        private readonly ILibroRepository repository;

        public LibroService(ILibroRepository repository)
        {
            this.repository = repository;
        }

        public List<Libro> ObtenerTodos()
        {
            return repository.ObtenerTodos();
        }

        public string Crear(LibroCreateDto dto, out Libro libro)
        {
            libro = null;

            if (string.IsNullOrWhiteSpace(dto.Titulo))
            {
                return "El titulo es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(dto.Autor))
            {
                return "El autor es obligatorio.";
            }

            if (dto.AnioPublicacion < 1 || dto.AnioPublicacion > DateTime.Now.Year)
            {
                return "El anio de publicacion no es valido.";
            }

            libro = new Libro
            {
                Titulo = dto.Titulo.Trim(),
                Autor = dto.Autor.Trim(),
                AnioPublicacion = dto.AnioPublicacion,
                Disponible = true
            };

            repository.Agregar(libro);
            return string.Empty;
        }
    }
}
