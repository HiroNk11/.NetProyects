using System;
using System.Collections.Generic;

namespace AgendaContactosSqlServer
{
    internal class Program
    {
        private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=AgendaContactosDb;Integrated Security=True;";

        private static readonly ContactoRepository Repository = new ContactoRepository(ConnectionString);

        private static void Main(string[] args)
        {
            try
            {
                Repository.CrearTablaSiNoExiste();
                EjecutarMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo conectar con SQL Server.");
                Console.WriteLine(ex.Message);
            }
        }

        private static void EjecutarMenu()
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=== Agenda de contactos SQL Server ===");
                Console.WriteLine("1. Listar contactos");
                Console.WriteLine("2. Agregar contacto");
                Console.WriteLine("3. Eliminar contacto");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opcion: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Listar();
                        break;
                    case "2":
                        Agregar();
                        break;
                    case "3":
                        Eliminar();
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

        private static void Listar()
        {
            List<Contacto> contactos = Repository.ObtenerTodos();

            Console.WriteLine();
            Console.WriteLine("Contactos:");

            if (contactos.Count == 0)
            {
                Console.WriteLine("No hay contactos cargados.");
                return;
            }

            foreach (Contacto contacto in contactos)
            {
                Console.WriteLine(contacto.ObtenerResumen());
            }
        }

        private static void Agregar()
        {
            Contacto contacto = new Contacto
            {
                Nombre = PedirTexto("Nombre: "),
                Telefono = PedirTexto("Telefono: "),
                Email = PedirTexto("Email: ")
            };

            Repository.Agregar(contacto);
            Console.WriteLine("Contacto agregado correctamente.");
        }

        private static void Eliminar()
        {
            Console.Write("Ingrese el Id del contacto a eliminar: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Id invalido.");
                return;
            }

            Repository.Eliminar(id);
            Console.WriteLine("Contacto eliminado si existia en la base de datos.");
        }

        private static string PedirTexto(string mensaje)
        {
            string valor;

            do
            {
                Console.Write(mensaje);
                valor = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(valor));

            return valor;
        }
    }
}
