using System;
using System.Collections.Generic;
using TicketsSoporteCleanArchitecture.Application.Services;
using TicketsSoporteCleanArchitecture.Domain;

namespace TicketsSoporteCleanArchitecture.Presentation
{
    internal class ConsoleMenu
    {
        private readonly TicketService _ticketService;

        public ConsoleMenu(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public void Ejecutar()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=== Tickets de soporte ===");
                Console.WriteLine("1. Crear ticket");
                Console.WriteLine("2. Asignar tecnico");
                Console.WriteLine("3. Resolver ticket");
                Console.WriteLine("4. Listar abiertos");
                Console.WriteLine("5. Listar por prioridad");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opcion: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CrearTicket();
                        break;
                    case "2":
                        AsignarTecnico();
                        break;
                    case "3":
                        ResolverTicket();
                        break;
                    case "4":
                        MostrarTickets(_ticketService.ListarAbiertos());
                        break;
                    case "5":
                        MostrarTickets(_ticketService.ListarPorPrioridad(PedirPrioridad()));
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opcion invalida.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        private void CrearTicket()
        {
            try
            {
                int id = PedirEntero("Id: ");
                Console.Write("Cliente: ");
                string cliente = Console.ReadLine();
                Console.Write("Descripcion: ");
                string descripcion = Console.ReadLine();
                PrioridadTicket prioridad = PedirPrioridad();

                Ticket ticket = new Ticket(id, cliente, descripcion, prioridad);
                _ticketService.CrearTicket(ticket, out string mensaje);
                Console.WriteLine(mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo crear el ticket: {ex.Message}");
            }
        }

        private void AsignarTecnico()
        {
            try
            {
                int id = PedirEntero("Id del ticket: ");
                Console.Write("Tecnico: ");
                string tecnico = Console.ReadLine();

                _ticketService.AsignarTecnico(id, tecnico, out string mensaje);
                Console.WriteLine(mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo asignar el tecnico: {ex.Message}");
            }
        }

        private void ResolverTicket()
        {
            int id = PedirEntero("Id del ticket: ");
            _ticketService.ResolverTicket(id, out string mensaje);
            Console.WriteLine(mensaje);
        }

        private static void MostrarTickets(IReadOnlyList<Ticket> tickets)
        {
            if (tickets.Count == 0)
            {
                Console.WriteLine("No hay tickets para mostrar.");
                return;
            }

            foreach (Ticket ticket in tickets)
            {
                string tecnico = string.IsNullOrWhiteSpace(ticket.TecnicoAsignado) ? "Sin asignar" : ticket.TecnicoAsignado;
                Console.WriteLine($"{ticket.Id}. {ticket.Cliente} | {ticket.Prioridad} | {ticket.Estado} | {tecnico}");
                Console.WriteLine($"   {ticket.Descripcion}");
            }
        }

        private static PrioridadTicket PedirPrioridad()
        {
            Console.WriteLine("Prioridad: 1-Baja 2-Media 3-Alta 4-Critica");
            int valor = PedirEntero("Seleccione prioridad: ");

            return Enum.IsDefined(typeof(PrioridadTicket), valor)
                ? (PrioridadTicket)valor
                : PrioridadTicket.Media;
        }

        private static int PedirEntero(string mensaje)
        {
            int valor;

            while (true)
            {
                Console.Write(mensaje);

                if (int.TryParse(Console.ReadLine(), out valor) && valor > 0)
                {
                    return valor;
                }

                Console.WriteLine("Ingrese un numero mayor a cero.");
            }
        }
    }
}
