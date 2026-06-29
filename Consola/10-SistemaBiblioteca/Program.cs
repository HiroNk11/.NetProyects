using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaBiblioteca
{
    internal class Program
    {
        private static readonly List<Libro> Libros = new List<Libro>();
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
                        AgregarLibro();
                        break;
                    case "2":
                        ListarLibros();
                        break;
                    case "3":
                        BuscarLibro();
                        break;
                    case "4":
                        PrestarLibro();
                        break;
                    case "5":
                        DevolverLibro();
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
            Console.WriteLine("=== Sistema de biblioteca ===");
            Console.WriteLine("1. Agregar libro");
            Console.WriteLine("2. Listar libros");
            Console.WriteLine("3. Buscar libro por titulo o autor");
            Console.WriteLine("4. Prestar libro");
            Console.WriteLine("5. Devolver libro");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void AgregarLibro()
        {
            Libro libro = new Libro
            {
                Id = proximoId,
                Titulo = PedirTexto("Ingrese el titulo: "),
                Autor = PedirTexto("Ingrese el autor: "),
                AnioPublicacion = PedirAnio("Ingrese el anio de publicacion: ")
            };

            Libros.Add(libro);
            proximoId++;

            Console.WriteLine("Libro agregado correctamente.");
        }

        private static void ListarLibros()
        {
            Console.WriteLine();

            if (Libros.Count == 0)
            {
                Console.WriteLine("No hay libros cargados.");
                return;
            }

            Console.WriteLine("Listado de libros:");

            foreach (Libro libro in Libros)
            {
                Console.WriteLine(libro.ObtenerResumen());
            }
        }

        private static void BuscarLibro()
        {
            if (Libros.Count == 0)
            {
                Console.WriteLine("No hay libros cargados.");
                return;
            }

            string busqueda = PedirTexto("Ingrese titulo, autor o parte del texto: ");
            List<Libro> resultados = Libros
                .Where(libro =>
                    libro.Titulo.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    libro.Autor.IndexOf(busqueda, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            Console.WriteLine();

            if (resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron libros.");
                return;
            }

            Console.WriteLine("Resultados:");

            foreach (Libro libro in resultados)
            {
                Console.WriteLine(libro.ObtenerResumen());
            }
        }

        private static void PrestarLibro()
        {
            if (Libros.Count == 0)
            {
                Console.WriteLine("No hay libros cargados.");
                return;
            }

            ListarLibros();
            int id = PedirEntero("Ingrese el ID del libro a prestar: ");
            Libro libro = BuscarLibroPorId(id);

            if (libro == null)
            {
                Console.WriteLine("No se encontro un libro con ese ID.");
                return;
            }

            if (!libro.Prestar())
            {
                Console.WriteLine("El libro ya se encuentra prestado.");
                return;
            }

            Console.WriteLine("Libro prestado correctamente.");
        }

        private static void DevolverLibro()
        {
            if (Libros.Count == 0)
            {
                Console.WriteLine("No hay libros cargados.");
                return;
            }

            ListarLibros();
            int id = PedirEntero("Ingrese el ID del libro a devolver: ");
            Libro libro = BuscarLibroPorId(id);

            if (libro == null)
            {
                Console.WriteLine("No se encontro un libro con ese ID.");
                return;
            }

            if (!libro.Devolver())
            {
                Console.WriteLine("El libro ya estaba disponible.");
                return;
            }

            Console.WriteLine("Libro devuelto correctamente.");
        }

        private static Libro BuscarLibroPorId(int id)
        {
            return Libros.FirstOrDefault(libro => libro.Id == id);
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

        private static int PedirAnio(string mensaje)
        {
            int anio;
            bool esValido;
            int anioActual = DateTime.Now.Year;

            do
            {
                Console.Write(mensaje);
                esValido = int.TryParse(Console.ReadLine(), out anio);

                if (!esValido || anio < 1 || anio > anioActual)
                {
                    Console.WriteLine($"Ingrese un anio valido entre 1 y {anioActual}.");
                    esValido = false;
                }
            }
            while (!esValido);

            return anio;
        }
    }
}
