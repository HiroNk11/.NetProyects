using System.Collections.Generic;
using System.Linq;

namespace GeneradorReportes
{
    internal class ReporteVentas
    {
        public ReporteVentas(List<Venta> ventas)
        {
            Ventas = ventas;
        }

        public List<Venta> Ventas { get; }
        public decimal TotalVentas => Ventas.Sum(v => v.Total);
    }
}
