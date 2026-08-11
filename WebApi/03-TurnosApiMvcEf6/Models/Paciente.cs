using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TurnosApiMvcEf6.Models
{
    public class Paciente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(20)]
        public string Documento { get; set; }

        public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
