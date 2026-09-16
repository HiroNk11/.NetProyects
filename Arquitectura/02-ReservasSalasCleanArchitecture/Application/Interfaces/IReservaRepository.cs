using System.Collections.Generic;
using ReservasSalasCleanArchitecture.Domain;

namespace ReservasSalasCleanArchitecture.Application.Interfaces
{
    internal interface IReservaRepository
    {
        void Agregar(Reserva reserva);

        IReadOnlyList<Reserva> Listar();

        bool Existe(int id);
    }
}
