using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApiMvcEf6.Models
{
    public class Socio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(120)]
        public string Email { get; set; }

        public bool Activo { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
