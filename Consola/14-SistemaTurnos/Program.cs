using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SistemaTurnos
{
    internal class Program
    {
        private static readonly List<Turno> Turnos = new List<Turno>();
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
                        AgendarTurno();
                        break;
                    case "2":
                        ListarTurnos();
                        break;
                    case "3":
                        BuscarTurnosPorPaciente();
                        break;
                    case "4":
                        CancelarTurno();
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
            Console.WriteLine("=== Sistema de turnos ===");
            Console.WriteLine("1. Agendar turno");
            Console.WriteLine("2. Listar turnos");
            Console.WriteLine("3. Buscar turnos por paciente");
            Console.WriteLine("4. Cancelar turno");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void AgendarTurno()
        {
            string paciente = PedirTexto("Ingrese el paciente: ");
            string profesional = PedirTexto("Ingrese el profesional: ");
            DateTime fechaHora = PedirFechaHora("Ingrese fecha y hora (dd/MM/yyyy HH:mm): ");

            if (fechaHora <= DateTime.Now)
            {
                Console.WriteLine("El turno debe ser para una fecha y hora futura.");
                return;
            }

            bool profesionalOcupado = Turnos.Any(turno =>
                !turno.EstaCancelado &&
                turno.Profesional.Equals(profesional, StringComparison.OrdinalIgnoreCase) &&
                turno.FechaHora == fechaHora);

            if (profesionalOcupado)
            {
                Console.WriteLine("El profesional ya tiene un turno en esa fecha y hora.");
                return;
            }

            Turno turnoNuevo = new Turno
            {
                Id = proximoId,
                Paciente = paciente,
                Profesional = profesional,
                FechaHora = fechaHora
            };

            Turnos.Add(turnoNuevo);
            proximoId++;

            Console.WriteLine("Turno agendado correctamente.");
            Console.WriteLine(turnoNuevo.ObtenerResumen());
        }

        private static void ListarTurnos()
        {
            Console.WriteLine();

            if (Turnos.Count == 0)
            {
                Console.WriteLine("No hay turnos registrados.");
                return;
            }

            foreach (Turno turno in Turnos.OrderBy(turno => turno.FechaHora))
            {
                Console.WriteLine(turno.ObtenerResumen());
            }
        }

        private static void BuscarTurnosPorPaciente()
        {
            if (Turnos.Count == 0)
            {
                Console.WriteLine("No hay turnos registrados.");
                return;
            }

            string busqueda = PedirTexto("Ingrese paciente o parte del nombre: ");
            List<Turno> resultados = Turnos
                .Where(turno => turno.Paciente.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(turno => turno.FechaHora)
                .ToList();

            Console.WriteLine();

            if (resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron turnos.");
                return;
            }

            foreach (Turno turno in resultados)
            {
                Console.WriteLine(turno.ObtenerResumen());
            }
        }

        private static void CancelarTurno()
        {
            if (Turnos.Count == 0)
            {
                Console.WriteLine("No hay turnos para cancelar.");
                return;
            }

            ListarTurnos();
            int id = PedirEntero("Ingrese el ID del turno a cancelar: ");
            Turno turno = Turnos.FirstOrDefault(item => item.Id == id);

            if (turno == null)
            {
                Console.WriteLine("No se encontro un turno con ese ID.");
                return;
            }

            if (turno.EstaCancelado)
            {
                Console.WriteLine("El turno ya estaba cancelado.");
                return;
            }

            turno.Cancelar();
            Console.WriteLine("Turno cancelado correctamente.");
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

        private static DateTime PedirFechaHora(string mensaje)
        {
            DateTime fechaHora;
            bool esValida;

            do
            {
                Console.Write(mensaje);
                esValida = DateTime.TryParseExact(
                    Console.ReadLine(),
                    "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fechaHora);

                if (!esValida)
                {
                    Console.WriteLine("Ingrese fecha y hora validas con formato dd/MM/yyyy HH:mm.");
                }
            }
            while (!esValida);

            return fechaHora;
        }
    }
}
