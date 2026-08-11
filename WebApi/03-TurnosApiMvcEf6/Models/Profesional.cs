using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TurnosApiMvcEf6.Models
{
    public class Profesional
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(80)]
        public string Especialidad { get; set; }

        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
