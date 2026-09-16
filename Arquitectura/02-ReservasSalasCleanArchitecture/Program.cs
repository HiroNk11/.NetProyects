using System;
using ReservasSalasCleanArchitecture.Application.Services;
using ReservasSalasCleanArchitecture.Domain;
using ReservasSalasCleanArchitecture.Infrastructure;
using ReservasSalasCleanArchitecture.Presentation;

namespace ReservasSalasCleanArchitecture
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            InMemoryReservaRepository repository = new InMemoryReservaRepository();
            CargarDatosIniciales(repository);

            ReservaService service = new ReservaService(repository);
            ConsoleMenu menu = new ConsoleMenu(service);
            menu.Ejecutar();
        }

        private static void CargarDatosIniciales(InMemoryReservaRepository repository)
        {
            repository.Agregar(new Reserva(1, "Sala Norte", "Equipo Comercial", DateTime.Today.AddHours(9), DateTime.Today.AddHours(10)));
            repository.Agregar(new Reserva(2, "Sala Sur", "Equipo Desarrollo", DateTime.Today.AddHours(11), DateTime.Today.AddHours(12)));
        }
    }
}
