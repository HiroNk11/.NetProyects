using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorTareas
{
    internal class Program
    {
        private static readonly List<Tarea> Tareas = new List<Tarea>();
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
                        AgregarTarea();
                        break;
                    case "2":
                        ListarTareas();
                        break;
                    case "3":
                        MarcarTareaComoCompletada();
                        break;
                    case "4":
                        EliminarTarea();
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
            Console.WriteLine("=== Gestor de tareas ===");
            Console.WriteLine("1. Agregar tarea");
            Console.WriteLine("2. Listar tareas");
            Console.WriteLine("3. Marcar tarea como completada");
            Console.WriteLine("4. Eliminar tarea");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void AgregarTarea()
        {
            Console.Write("Ingrese el titulo de la tarea: ");
            string titulo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(titulo))
            {
                Console.WriteLine("El titulo no puede estar vacio.");
                return;
            }

            Tarea nuevaTarea = new Tarea
            {
                Id = proximoId,
                Titulo = titulo.Trim(),
                EstaCompletada = false
            };

            Tareas.Add(nuevaTarea);
            proximoId++;

            Console.WriteLine("Tarea agregada correctamente.");
        }

        private static void ListarTareas()
        {
            Console.WriteLine();

            if (Tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas cargadas.");
                return;
            }

            Console.WriteLine("Listado de tareas:");

            foreach (Tarea tarea in Tareas)
            {
                Console.WriteLine($"{tarea.Id}. [{tarea.ObtenerEstado()}] {tarea.Titulo}");
            }
        }

        private static void MarcarTareaComoCompletada()
        {
            if (Tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas para completar.");
                return;
            }

            ListarTareas();
            int id = PedirId("Ingrese el ID de la tarea a completar: ");
            Tarea tarea = BuscarTareaPorId(id);

            if (tarea == null)
            {
                Console.WriteLine("No se encontro una tarea con ese ID.");
                return;
            }

            if (tarea.EstaCompletada)
            {
                Console.WriteLine("La tarea ya estaba completada.");
                return;
            }

            tarea.EstaCompletada = true;
            Console.WriteLine("Tarea marcada como completada.");
        }

        private static void EliminarTarea()
        {
            if (Tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas para eliminar.");
                return;
            }

            ListarTareas();
            int id = PedirId("Ingrese el ID de la tarea a eliminar: ");
            Tarea tarea = BuscarTareaPorId(id);

            if (tarea == null)
            {
                Console.WriteLine("No se encontro una tarea con ese ID.");
                return;
            }

            Tareas.Remove(tarea);
            Console.WriteLine("Tarea eliminada correctamente.");
        }

        private static int PedirId(string mensaje)
        {
            int id;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out id);

                if (!esValido || id <= 0)
                {
                    Console.WriteLine("Ingrese un ID valido.");
                    esValido = false;
                }
            }
            while (!esValido);

            return id;
        }

        private static Tarea BuscarTareaPorId(int id)
        {
            return Tareas.FirstOrDefault(tarea => tarea.Id == id);
        }
    }
}
