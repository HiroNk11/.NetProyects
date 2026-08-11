using System.Collections.Generic;
using System.Linq;
using AgendaContactosWinFormsMvc.Models;

namespace AgendaContactosWinFormsMvc.Repositories
{
    internal class ContactoRepository : IContactoRepository
    {
        private readonly List<Contacto> contactos = new List<Contacto>();
        private int proximoId = 1;

        public List<Contacto> ObtenerTodos()
        {
            return contactos.ToList();
        }

        public Contacto ObtenerPorId(int id)
        {
            return contactos.FirstOrDefault(contacto => contacto.Id == id);
        }

        public void Agregar(Contacto contacto)
        {
            contacto.Id = proximoId;
            proximoId++;
            contactos.Add(contacto);
        }

        public void Actualizar(Contacto contacto)
        {
            Contacto existente = ObtenerPorId(contacto.Id);

            if (existente == null)
            {
                return;
            }

            existente.Nombre = contacto.Nombre;
            existente.Telefono = contacto.Telefono;
            existente.Email = contacto.Email;
        }

        public void Eliminar(int id)
        {
            Contacto contacto = ObtenerPorId(id);

            if (contacto != null)
            {
                contactos.Remove(contacto);
            }
        }
    }
}
