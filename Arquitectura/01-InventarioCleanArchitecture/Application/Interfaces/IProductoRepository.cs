using System.Collections.Generic;
using InventarioCleanArchitecture.Domain;

namespace InventarioCleanArchitecture.Application.Interfaces
{
    internal interface IProductoRepository
    {
        void Agregar(Producto producto);

        Producto BuscarPorId(int id);

        IReadOnlyList<Producto> Listar();

        bool Existe(int id);
    }
}
