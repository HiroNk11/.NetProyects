using System;

namespace BibliotecaApiMvcEf6.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int SocioId { get; set; }
        public int LibroId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; }

        public virtual Socio Socio { get; set; }
        public virtual Libro Libro { get; set; }
    }
}
