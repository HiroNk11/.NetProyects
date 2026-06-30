using System;
using System.Collections.Generic;
using GestorTareasMvc.Models;

namespace GestorTareasMvc.Views
{
    internal class TareaView
    {
        public void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Gestor de tareas MVC ===");
            Console.WriteLine("1. Listar tareas");
            Console.WriteLine("2. Agregar tarea");
            Console.WriteLine("3. Completar tarea");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        public void MostrarTareas(List<Tarea> tareas)
        {
            Console.WriteLine();
            Console.WriteLine("Tareas:");

            if (tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas cargadas.");
                return;
            }

            foreach (Tarea tarea in tareas)
            {
                string estado = tarea.EstaCompletada ? "Completada" : "Pendiente";
                Console.WriteLine($"{tarea.Id}. [{estado}] {tarea.Descripcion}");
            }
        }

        public string PedirDescripcion()
        {
            string descripcion;

            do
            {
                Console.Write("Descripcion: ");
                descripcion = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(descripcion));

            return descripcion;
        }

        public int PedirId()
        {
            Console.Write("Id de la tarea: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                return 0;
            }

            return id;
        }

        public void MostrarMensaje(string mensaje)
        {
            Console.WriteLine(mensaje);
        }
    }
}
