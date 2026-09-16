using System;
using PruebasUnitariasBasicas.Testing;
using PruebasUnitariasBasicas.Tests;

namespace PruebasUnitariasBasicas
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CalculadoraTests tests = new CalculadoraTests();
            TestRunner runner = new TestRunner();

            runner.Add(tests.Sumar_DeberiaRetornarTotal);
            runner.Add(tests.Dividir_DeberiaRetornarCociente);
            runner.Add(tests.Dividir_CuandoDivisorEsCero_DeberiaLanzarExcepcion);
            runner.Add(tests.CalcularPorcentaje_DeberiaRetornarImporte);

            Console.WriteLine("=== Pruebas unitarias basicas ===");
            runner.Run();

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
