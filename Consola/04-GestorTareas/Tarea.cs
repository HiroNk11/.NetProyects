namespace GestorTareas
{
    internal class Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public bool EstaCompletada { get; set; }

        public string ObtenerEstado()
        {
            return EstaCompletada ? "Completada" : "Pendiente";
        }
    }
}
