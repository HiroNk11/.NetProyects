using System;

namespace TurnosApiMvcEf6.Dtos
{
    public class TurnoResumenDto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int DuracionMinutos { get; set; }
        public string Paciente { get; set; }
        public string Profesional { get; set; }
        public string Especialidad { get; set; }
        public string Estado { get; set; }
        public string Motivo { get; set; }
    }
}
