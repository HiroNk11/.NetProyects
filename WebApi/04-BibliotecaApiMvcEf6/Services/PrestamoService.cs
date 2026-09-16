using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaApiMvcEf6.Dtos;
using BibliotecaApiMvcEf6.Models;
using BibliotecaApiMvcEf6.Repositories;

namespace BibliotecaApiMvcEf6.Services
{
    public class PrestamoService
    {
        private const string EstadoActivo = "Activo";
        private const string EstadoDevuelto = "Devuelto";

        private readonly ISocioRepository socioRepository;
        private readonly ILibroRepository libroRepository;
        private readonly IPrestamoRepository prestamoRepository;

        public PrestamoService(ISocioRepository socioRepository, ILibroRepository libroRepository, IPrestamoRepository prestamoRepository)
        {
            this.socioRepository = socioRepository;
            this.libroRepository = libroRepository;
            this.prestamoRepository = prestamoRepository;
        }

        public List<PrestamoResumenDto> ObtenerTodos()
        {
            return prestamoRepository.ObtenerTodos().Select(MapearResumen).ToList();
        }

        public string Crear(PrestamoCreateDto dto, out Prestamo prestamo)
        {
            prestamo = null;
            Socio socio = socioRepository.ObtenerPorId(dto.SocioId);
            Libro libro = libroRepository.ObtenerPorId(dto.LibroId);

            if (socio == null || !socio.Activo)
            {
                return "Socio inexistente o inactivo.";
            }

            if (libro == null)
            {
                return "Libro inexistente.";
            }

            if (!libro.Disponible)
            {
                return "El libro no esta disponible.";
            }

            if (dto.DiasPrestamo <= 0 || dto.DiasPrestamo > 60)
            {
                return "Los dias de prestamo deben estar entre 1 y 60.";
            }

            prestamo = new Prestamo
            {
                SocioId = socio.Id,
                LibroId = libro.Id,
                Socio = socio,
                Libro = libro,
                FechaPrestamo = DateTime.Now,
                FechaVencimiento = DateTime.Now.Date.AddDays(dto.DiasPrestamo),
                Estado = EstadoActivo
            };

            libro.Disponible = false;
            libroRepository.Actualizar(libro);
            prestamoRepository.Agregar(prestamo);
            return string.Empty;
        }

        public bool Devolver(int id)
        {
            Prestamo prestamo = prestamoRepository.ObtenerPorId(id);

            if (prestamo == null || prestamo.Estado == EstadoDevuelto)
            {
                return false;
            }

            prestamo.Estado = EstadoDevuelto;
            prestamo.FechaDevolucion = DateTime.Now;
            prestamo.Libro.Disponible = true;

            libroRepository.Actualizar(prestamo.Libro);
            prestamoRepository.Actualizar(prestamo);
            return true;
        }

        private PrestamoResumenDto MapearResumen(Prestamo prestamo)
        {
            return new PrestamoResumenDto
            {
                Id = prestamo.Id,
                Socio = prestamo.Socio.Nombre,
                Libro = prestamo.Libro.Titulo,
                FechaPrestamo = prestamo.FechaPrestamo,
                FechaVencimiento = prestamo.FechaVencimiento,
                FechaDevolucion = prestamo.FechaDevolucion,
                Estado = prestamo.Estado
            };
        }
    }
}
