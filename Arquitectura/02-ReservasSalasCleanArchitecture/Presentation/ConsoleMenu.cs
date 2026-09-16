using System;
using System.Collections.Generic;
using ReservasSalasCleanArchitecture.Application.Services;
using ReservasSalasCleanArchitecture.Domain;

namespace ReservasSalasCleanArchitecture.Presentation
{
    internal class ConsoleMenu
    {
        private readonly ReservaService _reservaService;

        public ConsoleMenu(ReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        public void Ejecutar()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=== Reservas de salas ===");
                Console.WriteLine("1. Listar reservas");
                Console.WriteLine("2. Crear reserva");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opcion: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ListarReservas();
                        break;
                    case "2":
                        CrearReserva();
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

        private void CrearReserva()
        {
            try
            {
                int id = PedirEntero("Id: ");
                Console.Write("Sala: ");
                string sala = Console.ReadLine();
                Console.Write("Solicitante: ");
                string solicitante = Console.ReadLine();
                DateTime inicio = PedirFecha("Inicio (yyyy-MM-dd HH:mm): ");
                DateTime fin = PedirFecha("Fin (yyyy-MM-dd HH:mm): ");

                Reserva reserva = new Reserva(id, sala, solicitante, inicio, fin);
                bool resultado = _reservaService.CrearReserva(reserva, out string mensaje);

                Console.WriteLine(mensaje);

                if (!resultado)
                {
                    Console.WriteLine("Revise los datos e intente nuevamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo crear la reserva: {ex.Message}");
            }
        }

        private void ListarReservas()
        {
            IReadOnlyList<Reserva> reservas = _reservaService.ListarReservas();

            if (reservas.Count == 0)
            {
                Console.WriteLine("No hay reservas cargadas.");
                return;
            }

            foreach (Reserva reserva in reservas)
            {
                Console.WriteLine(
                    $"{reserva.Id}. {reserva.Sala} | {reserva.Solicitante} | {reserva.Inicio:yyyy-MM-dd HH:mm} a {reserva.Fin:HH:mm}");
            }
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

        private static DateTime PedirFecha(string mensaje)
        {
            DateTime valor;

            while (true)
            {
                Console.Write(mensaje);

                if (DateTime.TryParse(Console.ReadLine(), out valor))
                {
                    return valor;
                }

                Console.WriteLine("Ingrese una fecha valida.");
            }
        }
    }
}
