using System;

namespace ReservasHotel
{
    internal class Reserva
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public Habitacion Habitacion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }

        public int CalcularNoches()
        {
            return (FechaSalida - FechaIngreso).Days;
        }

        public decimal CalcularTotal()
        {
            return CalcularNoches() * Habitacion.PrecioPorNoche;
        }

        public string ObtenerResumen()
        {
            return $"{Id}. {Cliente} | Hab. {Habitacion.Numero} | {FechaIngreso:dd/MM/yyyy} al {FechaSalida:dd/MM/yyyy} | Total: ${CalcularTotal():0.00}";
        }
    }
}
