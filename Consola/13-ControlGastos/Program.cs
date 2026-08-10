using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ControlGastos
{
    internal class Program
    {
        private static readonly List<Gasto> Gastos = new List<Gasto>();
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
                        RegistrarGasto();
                        break;
                    case "2":
                        ListarGastos();
                        break;
                    case "3":
                        MostrarTotalGastado();
                        break;
                    case "4":
                        MostrarTotalPorCategoria();
                        break;
                    case "5":
                        EliminarGasto();
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
            Console.WriteLine("=== Control de gastos personales ===");
            Console.WriteLine("1. Registrar gasto");
            Console.WriteLine("2. Listar gastos");
            Console.WriteLine("3. Ver total gastado");
            Console.WriteLine("4. Ver total por categoria");
            Console.WriteLine("5. Eliminar gasto");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opcion: ");
        }

        private static void RegistrarGasto()
        {
            Gasto gasto = new Gasto
            {
                Id = proximoId,
                Fecha = PedirFecha("Ingrese la fecha (dd/MM/yyyy): "),
                Categoria = PedirTexto("Ingrese la categoria: "),
                Descripcion = PedirTexto("Ingrese la descripcion: "),
                Importe = PedirImporte("Ingrese el importe: ")
            };

            Gastos.Add(gasto);
            proximoId++;

            Console.WriteLine("Gasto registrado correctamente.");
        }

        private static void ListarGastos()
        {
            Console.WriteLine();

            if (Gastos.Count == 0)
            {
                Console.WriteLine("No hay gastos registrados.");
                return;
            }

            foreach (Gasto gasto in Gastos.OrderBy(gasto => gasto.Fecha))
            {
                Console.WriteLine(gasto.ObtenerResumen());
            }
        }

        private static void MostrarTotalGastado()
        {
            Console.WriteLine();

            if (Gastos.Count == 0)
            {
                Console.WriteLine("No hay gastos registrados.");
                return;
            }

            decimal total = Gastos.Sum(gasto => gasto.Importe);
            Console.WriteLine($"Total gastado: ${total:0.00}");
        }

        private static void MostrarTotalPorCategoria()
        {
            Console.WriteLine();

            if (Gastos.Count == 0)
            {
                Console.WriteLine("No hay gastos registrados.");
                return;
            }

            var totales = Gastos
                .GroupBy(gasto => gasto.Categoria)
                .Select(grupo => new { Categoria = grupo.Key, Total = grupo.Sum(gasto => gasto.Importe) })
                .OrderByDescending(item => item.Total);

            foreach (var item in totales)
            {
                Console.WriteLine($"{item.Categoria}: ${item.Total:0.00}");
            }
        }

        private static void EliminarGasto()
        {
            if (Gastos.Count == 0)
            {
                Console.WriteLine("No hay gastos para eliminar.");
                return;
            }

            ListarGastos();
            int id = PedirEntero("Ingrese el ID del gasto a eliminar: ");
            Gasto gasto = Gastos.FirstOrDefault(item => item.Id == id);

            if (gasto == null)
            {
                Console.WriteLine("No se encontro un gasto con ese ID.");
                return;
            }

            Gastos.Remove(gasto);
            Console.WriteLine("Gasto eliminado correctamente.");
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

        private static decimal PedirImporte(string mensaje)
        {
            decimal importe;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                esValido = decimal.TryParse(Console.ReadLine(), out importe);

                if (!esValido || importe <= 0)
                {
                    Console.WriteLine("Ingrese un importe mayor a cero.");
                    esValido = false;
                }
            }
            while (!esValido);

            return importe;
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
