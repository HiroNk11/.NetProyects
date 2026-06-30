using GestorTareasMvc.Models;
using GestorTareasMvc.Views;

namespace GestorTareasMvc.Controllers
{
    internal class TareaController
    {
        private readonly TareaRepository _repository;
        private readonly TareaView _view;

        public TareaController(TareaRepository repository, TareaView view)
        {
            _repository = repository;
            _view = view;
        }

        public void Listar()
        {
            _view.MostrarTareas(_repository.ObtenerTodas());
        }

        public void Agregar()
        {
            string descripcion = _view.PedirDescripcion();
            _repository.Agregar(descripcion);
            _view.MostrarMensaje("Tarea agregada correctamente.");
        }

        public void Completar()
        {
            int id = _view.PedirId();
            bool completada = _repository.Completar(id);

            _view.MostrarMensaje(completada ? "Tarea completada." : "No se encontro una tarea con ese Id.");
        }
    }
}
