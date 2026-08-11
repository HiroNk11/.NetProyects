using System.Collections.Generic;
using SistemaVentasWinFormsMvc.Models;

namespace SistemaVentasWinFormsMvc.Repositories
{
    internal interface IVentaRepository
    {
        List<Venta> ObtenerTodas();
        void Agregar(Venta venta);
    }
}
