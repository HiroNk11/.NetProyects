using System.Collections.Generic;
using SistemaVentasWinFormsMvc.Models;

namespace SistemaVentasWinFormsMvc.Repositories
{
    internal interface IClienteRepository
    {
        List<Cliente> ObtenerTodos();
        void Agregar(Cliente cliente);
    }
}
