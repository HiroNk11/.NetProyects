using System;

namespace BibliotecaApiMvcEf6.Dtos
{
    public class PrestamoResumenDto
    {
        public int Id { get; set; }
        public string Socio { get; set; }
        public string Libro { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; }
    }
}
