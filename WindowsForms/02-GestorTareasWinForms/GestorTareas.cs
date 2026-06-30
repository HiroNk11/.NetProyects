using System.Collections.Generic;
using System.Linq;

namespace GestorTareasWinForms
{
    internal class GestorTareas
    {
        private readonly List<Tarea> _tareas = new List<Tarea>();
        private int _proximoId = 1;

        public List<Tarea> ObtenerTodas()
        {
            return _tareas.ToList();
        }

        public void Agregar(string descripcion)
        {
            _tareas.Add(new Tarea(_proximoId, descripcion));
            _proximoId++;
        }

        public bool Completar(int id)
        {
            Tarea tarea = _tareas.FirstOrDefault(t => t.Id == id);

            if (tarea == null)
            {
                return false;
            }

            tarea.Completar();
            return true;
        }

        public bool Eliminar(int id)
        {
            Tarea tarea = _tareas.FirstOrDefault(t => t.Id == id);

            if (tarea == null)
            {
                return false;
            }

            _tareas.Remove(tarea);
            return true;
        }
    }
}
