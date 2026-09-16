using TicketsSoporteCleanArchitecture.Application.Services;
using TicketsSoporteCleanArchitecture.Domain;
using TicketsSoporteCleanArchitecture.Infrastructure;
using TicketsSoporteCleanArchitecture.Presentation;

namespace TicketsSoporteCleanArchitecture
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            InMemoryTicketRepository repository = new InMemoryTicketRepository();
            CargarDatosIniciales(repository);

            TicketService service = new TicketService(repository);
            ConsoleMenu menu = new ConsoleMenu(service);
            menu.Ejecutar();
        }

        private static void CargarDatosIniciales(InMemoryTicketRepository repository)
        {
            repository.Agregar(new Ticket(1, "Estudio Contable Norte", "No se puede emitir una factura.", PrioridadTicket.Alta));
            repository.Agregar(new Ticket(2, "Clinica Central", "El reporte mensual muestra totales incorrectos.", PrioridadTicket.Critica));
            repository.Agregar(new Ticket(3, "Tienda Online Sur", "Solicitud de cambio de logo en comprobantes.", PrioridadTicket.Baja));
        }
    }
}
