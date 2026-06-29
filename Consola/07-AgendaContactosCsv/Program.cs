using System;
using System.Collections.Generic;
using System.Linq;

namespace AgendaContactosCsv
{
    internal class Program
    {
        private static List<Contacto> contactos = new List<Contacto>();
        private static int proximoId = 1;

        private static void Main(string[] args)
        {
            contactos = ArchivoContactos.Cargar();
            proximoId = ObtenerProximoId();

            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarContacto();
                        break;
                    case "2":
                        ListarContactos();
                        break;
                    case "3":
                        BuscarContacto();
                        break;
                    case "4":
                        EditarContacto();
                        break;
                    case "5":
                        EliminarContacto();
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
            Console.WriteLine("=== Agenda de contactos con CSV ===");
            Console.WriteLine("1. Agregar contacto");
            Console.WriteLine("2. Listar contactos");
            Console.WriteLine("3. Buscar contacto por nombre");
            Console.WriteLine("4. Editar contacto");
            Console.WriteLine("5. Eliminar contacto");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void AgregarContacto()
        {
            Contacto contacto = new Contacto
            {
                Id = proximoId,
                Nombre = PedirTexto("Ingrese el nombre: "),
                Telefono = PedirTexto("Ingrese el telefono: "),
                Email = PedirEmail("Ingrese el email: ")
            };

            contactos.Add(contacto);
            proximoId++;
            GuardarCambios();

            Console.WriteLine("Contacto agregado correctamente.");
        }

        private static void ListarContactos()
        {
            Console.WriteLine();

            if (contactos.Count == 0)
            {
                Console.WriteLine("No hay contactos cargados.");
                return;
            }

            Console.WriteLine("Listado de contactos:");

            foreach (Contacto contacto in contactos)
            {
                Console.WriteLine(contacto.ObtenerResumen());
            }
        }

        private static void BuscarContacto()
        {
            if (contactos.Count == 0)
            {
                Console.WriteLine("No hay contactos cargados.");
                return;
            }

            string textoBusqueda = PedirTexto("Ingrese nombre o parte del nombre: ");
            List<Contacto> resultados = contactos
                .Where(contacto => contacto.Nombre.IndexOf(textoBusqueda, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            Console.WriteLine();

            if (resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron contactos.");
                return;
            }

            Console.WriteLine("Resultados:");

            foreach (Contacto contacto in resultados)
            {
                Console.WriteLine(contacto.ObtenerResumen());
            }
        }

        private static void EditarContacto()
        {
            if (contactos.Count == 0)
            {
                Console.WriteLine("No hay contactos para editar.");
                return;
            }

            ListarContactos();
            int id = PedirEntero("Ingrese el ID del contacto a editar: ");
            Contacto contacto = BuscarContactoPorId(id);

            if (contacto == null)
            {
                Console.WriteLine("No se encontro un contacto con ese ID.");
                return;
            }

            Console.WriteLine("Deje el campo vacio para conservar el valor actual.");

            string nombre = PedirTextoOpcional($"Nombre actual ({contacto.Nombre}): ");
            string telefono = PedirTextoOpcional($"Telefono actual ({contacto.Telefono}): ");
            string email = PedirEmailOpcional($"Email actual ({contacto.Email}): ");

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                contacto.Nombre = nombre;
            }

            if (!string.IsNullOrWhiteSpace(telefono))
            {
                contacto.Telefono = telefono;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                contacto.Email = email;
            }

            GuardarCambios();
            Console.WriteLine("Contacto editado correctamente.");
        }

        private static void EliminarContacto()
        {
            if (contactos.Count == 0)
            {
                Console.WriteLine("No hay contactos para eliminar.");
                return;
            }

            ListarContactos();
            int id = PedirEntero("Ingrese el ID del contacto a eliminar: ");
            Contacto contacto = BuscarContactoPorId(id);

            if (contacto == null)
            {
                Console.WriteLine("No se encontro un contacto con ese ID.");
                return;
            }

            contactos.Remove(contacto);
            GuardarCambios();

            Console.WriteLine("Contacto eliminado correctamente.");
        }

        private static void GuardarCambios()
        {
            ArchivoContactos.Guardar(contactos);
        }

        private static int ObtenerProximoId()
        {
            if (contactos.Count == 0)
            {
                return 1;
            }

            return contactos.Max(contacto => contacto.Id) + 1;
        }

        private static Contacto BuscarContactoPorId(int id)
        {
            return contactos.FirstOrDefault(contacto => contacto.Id == id);
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

        private static string PedirTextoOpcional(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine().Trim();
        }

        private static string PedirEmail(string mensaje)
        {
            string email;

            do
            {
                email = PedirTexto(mensaje);

                if (!EsEmailValido(email))
                {
                    Console.WriteLine("Ingrese un email valido.");
                }
            }
            while (!EsEmailValido(email));

            return email;
        }

        private static string PedirEmailOpcional(string mensaje)
        {
            string email;

            do
            {
                Console.Write(mensaje);
                email = Console.ReadLine().Trim();

                if (!string.IsNullOrWhiteSpace(email) && !EsEmailValido(email))
                {
                    Console.WriteLine("Ingrese un email valido o deje el campo vacio.");
                }
            }
            while (!string.IsNullOrWhiteSpace(email) && !EsEmailValido(email));

            return email;
        }

        private static bool EsEmailValido(string email)
        {
            return email.Contains("@") && email.Contains(".");
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
    }
}
