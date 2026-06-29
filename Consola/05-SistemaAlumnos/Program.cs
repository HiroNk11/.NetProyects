using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaAlumnos
{
    internal class Program
    {
        private static readonly List<Alumno> Alumnos = new List<Alumno>();

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
                        AgregarAlumno();
                        break;
                    case "2":
                        ListarAlumnos();
                        break;
                    case "3":
                        AgregarNota();
                        break;
                    case "4":
                        BuscarAlumno();
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
            Console.WriteLine("=== Sistema de alumnos ===");
            Console.WriteLine("1. Agregar alumno");
            Console.WriteLine("2. Listar alumnos");
            Console.WriteLine("3. Agregar nota a un alumno");
            Console.WriteLine("4. Buscar alumno por legajo");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void AgregarAlumno()
        {
            int legajo = PedirEntero("Ingrese el legajo: ");

            if (BuscarAlumnoPorLegajo(legajo) != null)
            {
                Console.WriteLine("Ya existe un alumno con ese legajo.");
                return;
            }

            string nombre = PedirTexto("Ingrese el nombre: ");
            string apellido = PedirTexto("Ingrese el apellido: ");

            Alumno alumno = new Alumno
            {
                Legajo = legajo,
                Nombre = nombre,
                Apellido = apellido
            };

            Alumnos.Add(alumno);
            Console.WriteLine("Alumno agregado correctamente.");
        }

        private static void ListarAlumnos()
        {
            Console.WriteLine();

            if (Alumnos.Count == 0)
            {
                Console.WriteLine("No hay alumnos cargados.");
                return;
            }

            Console.WriteLine("Listado de alumnos:");

            foreach (Alumno alumno in Alumnos)
            {
                MostrarAlumno(alumno);
            }
        }

        private static void AgregarNota()
        {
            if (Alumnos.Count == 0)
            {
                Console.WriteLine("No hay alumnos cargados.");
                return;
            }

            int legajo = PedirEntero("Ingrese el legajo del alumno: ");
            Alumno alumno = BuscarAlumnoPorLegajo(legajo);

            if (alumno == null)
            {
                Console.WriteLine("No se encontro un alumno con ese legajo.");
                return;
            }

            double nota = PedirNota("Ingrese la nota (0 a 10): ");
            alumno.Notas.Add(nota);

            Console.WriteLine("Nota agregada correctamente.");
        }

        private static void BuscarAlumno()
        {
            int legajo = PedirEntero("Ingrese el legajo a buscar: ");
            Alumno alumno = BuscarAlumnoPorLegajo(legajo);

            if (alumno == null)
            {
                Console.WriteLine("No se encontro un alumno con ese legajo.");
                return;
            }

            Console.WriteLine();
            MostrarAlumno(alumno);
        }

        private static void MostrarAlumno(Alumno alumno)
        {
            string notas = alumno.Notas.Count == 0 ? "Sin notas" : string.Join(", ", alumno.Notas.Select(nota => nota.ToString("0.##")));

            Console.WriteLine($"Legajo: {alumno.Legajo}");
            Console.WriteLine($"Alumno: {alumno.ObtenerNombreCompleto()}");
            Console.WriteLine($"Notas: {notas}");
            Console.WriteLine($"Promedio: {alumno.CalcularPromedio():0.##}");
            Console.WriteLine($"Estado: {alumno.ObtenerEstadoAcademico()}");
            Console.WriteLine("------------------------------");
        }

        private static Alumno BuscarAlumnoPorLegajo(int legajo)
        {
            return Alumnos.FirstOrDefault(alumno => alumno.Legajo == legajo);
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

        private static double PedirNota(string mensaje)
        {
            double nota;
            bool esValida;

            do
            {
                Console.Write(mensaje);
                esValida = double.TryParse(Console.ReadLine(), out nota);

                if (!esValida || nota < 0 || nota > 10)
                {
                    Console.WriteLine("Ingrese una nota valida entre 0 y 10.");
                    esValida = false;
                }
            }
            while (!esValida);

            return nota;
        }
    }
}
