using System;
using System.IO;

namespace GeneradorReportes
{
    internal class ArchivoReporteDestino : IReporteDestino
    {
        private readonly string _ruta;

        public ArchivoReporteDestino(string ruta)
        {
            _ruta = ruta;
        }

        public void Guardar(string contenido)
        {
            File.WriteAllText(_ruta, contenido);
            Console.WriteLine($"Reporte guardado en: {_ruta}");
        }
    }
}
