using System;

namespace TurnosApiMvcEf6.Dtos
{
    public class TurnoCreateDto
    {
        public int PacienteId { get; set; }
        public int ProfesionalId { get; set; }
        public DateTime FechaHora { get; set; }
        public int DuracionMinutos { get; set; }
        public string Motivo { get; set; }
    }
}
