using System;

namespace DashboardVentasPowerBI
{
    internal class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public string Canal { get; set; }
        public string Vendedor { get; set; }
    }
}
