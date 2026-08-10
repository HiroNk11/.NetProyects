namespace ReservasHotel
{
    internal class Habitacion
    {
        public int Numero { get; set; }
        public string Tipo { get; set; }
        public decimal PrecioPorNoche { get; set; }

        public string ObtenerResumen()
        {
            return $"Habitacion {Numero} | {Tipo} | ${PrecioPorNoche:0.00} por noche";
        }
    }
}
