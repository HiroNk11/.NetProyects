using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApiMvcEf6.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(100)]
        public string Autor { get; set; }

        public int AnioPublicacion { get; set; }
        public bool Disponible { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
