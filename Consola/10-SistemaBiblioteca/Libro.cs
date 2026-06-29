namespace SistemaBiblioteca
{
    internal class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int AnioPublicacion { get; set; }
        public bool EstaPrestado { get; private set; }

        public bool Prestar()
        {
            if (EstaPrestado)
            {
                return false;
            }

            EstaPrestado = true;
            return true;
        }

        public bool Devolver()
        {
            if (!EstaPrestado)
            {
                return false;
            }

            EstaPrestado = false;
            return true;
        }

        public string ObtenerEstado()
        {
            return EstaPrestado ? "Prestado" : "Disponible";
        }

        public string ObtenerResumen()
        {
            return $"{Id}. {Titulo} - {Autor} ({AnioPublicacion}) | {ObtenerEstado()}";
        }
    }
}
