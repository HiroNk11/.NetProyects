using System;

namespace SistemaTurnos
{
    internal class Turno
    {
        public int Id { get; set; }
        public string Paciente { get; set; }
        public string Profesional { get; set; }
        public DateTime FechaHora { get; set; }
        public bool EstaCancelado { get; private set; }

        public void Cancelar()
        {
            EstaCancelado = true;
        }

        public string ObtenerEstado()
        {
            return EstaCancelado ? "Cancelado" : "Activo";
        }

        public string ObtenerResumen()
        {
            return $"{Id}. {FechaHora:dd/MM/yyyy HH:mm} | {Paciente} con {Profesional} | {ObtenerEstado()}";
        }
    }
}
