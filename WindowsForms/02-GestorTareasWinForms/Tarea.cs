namespace GestorTareasWinForms
{
    internal class Tarea
    {
        public Tarea(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
            EstaCompletada = false;
        }

        public int Id { get; }
        public string Descripcion { get; }
        public bool EstaCompletada { get; private set; }
        public string Estado => EstaCompletada ? "Completada" : "Pendiente";

        public void Completar()
        {
            EstaCompletada = true;
        }
    }
}
