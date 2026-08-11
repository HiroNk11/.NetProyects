using System;
using System.ComponentModel.DataAnnotations;

namespace TurnosApiMvcEf6.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int ProfesionalId { get; set; }
        public DateTime FechaHora { get; set; }
        public int DuracionMinutos { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        [StringLength(200)]
        public string Motivo { get; set; }

        public DateTime FechaFin => FechaHora.AddMinutes(DuracionMinutos);
        public virtual Paciente Paciente { get; set; }
        public virtual Profesional Profesional { get; set; }
    }
}
