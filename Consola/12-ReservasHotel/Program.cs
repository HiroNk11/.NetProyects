using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ReservasHotel
{
    internal class Program
    {
        private static readonly List<Habitacion> Habitaciones = new List<Habitacion>
        {
            new Habitacion { Numero = 101, Tipo = "Simple", PrecioPorNoche = 25000 },
            new Habitacion { Numero = 102, Tipo = "Simple", PrecioPorNoche = 25000 },
            new Habitacion { Numero = 201, Tipo = "Doble", PrecioPorNoche = 40000 },
            new Habitacion { Numero = 202, Tipo = "Doble", PrecioPorNoche = 40000 },
            new Habitacion { Numero = 301, Tipo = "Suite", PrecioPorNoche = 75000 }
        };

        private static readonly List<Reserva> Reservas = new List<Reserva>();
        private static int proximoId = 1;

        private static void Main(string[] args)
        {
            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        CrearReserva();
                        break;
                    case "2":
                        ListarHabitaciones();
                        break;
                    case "3":
                        ListarReservas();
                        break;
                    case "4":
                        BuscarReservasPorCliente();
                        break;
                    case "5":
                        CancelarReserva();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opcion invalida. Intente nuevamente.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Programa finalizado.");
        }

        private static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Sistema de reservas de hotel ===");
            Console.WriteLine("1. Crear reserva");
            Console.WriteLine("2. Listar habitaciones");
            Console.WriteLine("3. Listar reservas");
            Console.WriteLine("4. Buscar reservas por cliente");
            Console.WriteLine("5. Cancelar reserva");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void CrearReserva()
        {
            string cliente = PedirTexto("Ingrese el nombre del cliente: ");
            DateTime fechaIngreso = PedirFecha("Ingrese la fecha de ingreso (dd/MM/yyyy): ");
            DateTime fechaSalida = PedirFecha("Ingrese la fecha de salida (dd/MM/yyyy): ");

            if (fechaSalida <= fechaIngreso)
            {
                Console.WriteLine("La fecha de salida debe ser posterior a la fecha de ingreso.");
                return;
            }

            List<Habitacion> disponibles = ObtenerHabitacionesDisponibles(fechaIngreso, fechaSalida);

            if (disponibles.Count == 0)
            {
                Console.WriteLine("No hay habitaciones disponibles para ese rango de fechas.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Habitaciones disponibles:");

            foreach (Habitacion habitacion in disponibles)
            {
                Console.WriteLine(habitacion.ObtenerResumen());
            }

            int numeroHabitacion = PedirEntero("Ingrese el numero de habitacion: ");
            Habitacion habitacionSeleccionada = disponibles.FirstOrDefault(habitacion => habitacion.Numero == numeroHabitacion);

            if (habitacionSeleccionada == null)
            {
                Console.WriteLine("La habitacion seleccionada no esta disponible.");
                return;
            }

            Reserva reserva = new Reserva
            {
                Id = proximoId,
                Cliente = cliente,
                Habitacion = habitacionSeleccionada,
                FechaIngreso = fechaIngreso,
                FechaSalida = fechaSalida
            };

            Reservas.Add(reserva);
            proximoId++;

            Console.WriteLine("Reserva creada correctamente.");
            Console.WriteLine(reserva.ObtenerResumen());
        }

        private static void ListarHabitaciones()
        {
            Console.WriteLine();
            Console.WriteLine("Habitaciones del hotel:");

            foreach (Habitacion habitacion in Habitaciones)
            {
                Console.WriteLine(habitacion.ObtenerResumen());
            }
        }

        private static void ListarReservas()
        {
            Console.WriteLine();

            if (Reservas.Count == 0)
            {
                Console.WriteLine("No hay reservas registradas.");
                return;
            }

            Console.WriteLine("Reservas registradas:");

            foreach (Reserva reserva in Reservas)
            {
                Console.WriteLine(reserva.ObtenerResumen());
            }
        }

        private static void BuscarReservasPorCliente()
        {
            if (Reservas.Count == 0)
            {
                Console.WriteLine("No hay reservas registradas.");
                return;
            }

            string busqueda = PedirTexto("Ingrese cliente o parte del nombre: ");
            List<Reserva> resultados = Reservas
                .Where(reserva => reserva.Cliente.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            Console.WriteLine();

            if (resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron reservas.");
                return;
            }

            foreach (Reserva reserva in resultados)
            {
                Console.WriteLine(reserva.ObtenerResumen());
            }
        }

        private static void CancelarReserva()
        {
            if (Reservas.Count == 0)
            {
                Console.WriteLine("No hay reservas para cancelar.");
                return;
            }

            ListarReservas();
            int id = PedirEntero("Ingrese el ID de la reserva a cancelar: ");
            Reserva reserva = Reservas.FirstOrDefault(item => item.Id == id);

            if (reserva == null)
            {
                Console.WriteLine("No se encontro una reserva con ese ID.");
                return;
            }

            Reservas.Remove(reserva);
            Console.WriteLine("Reserva cancelada correctamente.");
        }

        private static List<Habitacion> ObtenerHabitacionesDisponibles(DateTime fechaIngreso, DateTime fechaSalida)
        {
            return Habitaciones
                .Where(habitacion => !Reservas.Any(reserva =>
                    reserva.Habitacion.Numero == habitacion.Numero &&
                    FechasSeSuperponen(fechaIngreso, fechaSalida, reserva.FechaIngreso, reserva.FechaSalida)))
                .ToList();
        }

        private static bool FechasSeSuperponen(DateTime inicioNueva, DateTime finNueva, DateTime inicioExistente, DateTime finExistente)
        {
            return inicioNueva < finExistente && finNueva > inicioExistente;
        }

        private static string PedirTexto(string mensaje)
        {
            string texto;

            do
            {
                Console.Write(mensaje);
                texto = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    Console.WriteLine("El valor no puede estar vacio.");
                }
            }
            while (string.IsNullOrWhiteSpace(texto));

            return texto.Trim();
        }

        private static int PedirEntero(string mensaje)
        {
            int numero;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out numero);

                if (!esValido || numero <= 0)
                {
                    Console.WriteLine("Ingrese un numero entero mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return numero;
        }

        private static DateTime PedirFecha(string mensaje)
        {
            DateTime fecha;
            bool esValida;

            do
            {
                Console.Write(mensaje);
                esValida = DateTime.TryParseExact(
                    Console.ReadLine(),
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fecha);

                if (!esValida)
                {
                    Console.WriteLine("Ingrese una fecha valida con formato dd/MM/yyyy.");
                }
            }
            while (!esValida);

            return fecha;
        }
    }
}
