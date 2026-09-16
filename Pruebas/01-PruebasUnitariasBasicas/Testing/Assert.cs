using System;

namespace PruebasUnitariasBasicas.Testing
{
    internal static class Assert
    {
        public static void AreEqual<T>(T expected, T actual, string testName)
        {
            if (!Equals(expected, actual))
            {
                throw new Exception($"{testName}: esperado {expected}, obtenido {actual}.");
            }
        }

        public static void Throws<TException>(Action action, string testName)
            where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                return;
            }

            throw new Exception($"{testName}: se esperaba una excepcion {typeof(TException).Name}.");
        }
    }
}
