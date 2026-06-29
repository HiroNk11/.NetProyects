using System.Collections.Generic;
using System.Linq;

namespace SistemaAlumnos
{
    internal class Alumno
    {
        public int Legajo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public List<double> Notas { get; set; } = new List<double>();

        public string ObtenerNombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        public double CalcularPromedio()
        {
            if (Notas.Count == 0)
            {
                return 0;
            }

            return Notas.Average();
        }

        public string ObtenerEstadoAcademico()
        {
            if (Notas.Count == 0)
            {
                return "Sin notas";
            }

            return CalcularPromedio() >= 6 ? "Aprobado" : "Desaprobado";
        }
    }
}
