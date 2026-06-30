using System;
using GestorTareasMvc.Controllers;
using GestorTareasMvc.Models;
using GestorTareasMvc.Views;

namespace GestorTareasMvc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TareaRepository repository = new TareaRepository();
            TareaView view = new TareaView();
            TareaController controller = new TareaController(repository, view);

            bool continuar = true;

            while (continuar)
            {
                view.MostrarMenu();

                switch (Console.ReadLine())
                {
                    case "1":
                        controller.Listar();
                        break;
                    case "2":
                        controller.Agregar();
                        break;
                    case "3":
                        controller.Completar();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        view.MostrarMensaje("Opcion invalida.");
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
    }
}
