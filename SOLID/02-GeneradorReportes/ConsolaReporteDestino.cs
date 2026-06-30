using System;

namespace GeneradorReportes
{
    internal class ConsolaReporteDestino : IReporteDestino
    {
        public void Guardar(string contenido)
        {
            Console.WriteLine(contenido);
        }
    }
}
