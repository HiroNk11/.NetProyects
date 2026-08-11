using System.Collections.Generic;
using AgendaContactosWinFormsMvc.Models;

namespace AgendaContactosWinFormsMvc.Repositories
{
    internal interface IContactoRepository
    {
        List<Contacto> ObtenerTodos();
        Contacto ObtenerPorId(int id);
        void Agregar(Contacto contacto);
        void Actualizar(Contacto contacto);
        void Eliminar(int id);
    }
}
