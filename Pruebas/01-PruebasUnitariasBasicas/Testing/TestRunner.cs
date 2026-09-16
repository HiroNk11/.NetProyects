using System;
using System.Collections.Generic;

namespace PruebasUnitariasBasicas.Testing
{
    internal class TestRunner
    {
        private readonly List<Action> _tests = new List<Action>();

        public void Add(Action test)
        {
            _tests.Add(test);
        }

        public void Run()
        {
            int passed = 0;
            int failed = 0;

            foreach (Action test in _tests)
            {
                try
                {
                    test();
                    passed++;
                    Console.WriteLine($"OK - {test.Method.Name}");
                }
                catch (Exception ex)
                {
                    failed++;
                    Console.WriteLine($"ERROR - {test.Method.Name}: {ex.Message}");
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Resultado: {passed} OK, {failed} ERROR.");
        }
    }
}
