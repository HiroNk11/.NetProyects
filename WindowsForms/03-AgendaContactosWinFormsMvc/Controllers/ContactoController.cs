using System;
using System.Collections.Generic;
using System.Linq;
using AgendaContactosWinFormsMvc.Models;
using AgendaContactosWinFormsMvc.Repositories;
using AgendaContactosWinFormsMvc.Services;

namespace AgendaContactosWinFormsMvc.Controllers
{
    internal class ContactoController
    {
        private readonly IContactoRepository repository;
        private readonly ContactoValidator validator;

        public ContactoController(IContactoRepository repository, ContactoValidator validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        public List<Contacto> ObtenerTodos()
        {
            return repository.ObtenerTodos();
        }

        public List<Contacto> Buscar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return ObtenerTodos();
            }

            return repository.ObtenerTodos()
                .Where(contacto => contacto.Nombre.IndexOf(texto, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        public string Guardar(Contacto contacto)
        {
            string error = validator.Validar(contacto);

            if (!string.IsNullOrWhiteSpace(error))
            {
                return error;
            }

            if (contacto.Id == 0)
            {
                repository.Agregar(contacto);
            }
            else
            {
                repository.Actualizar(contacto);
            }

            return string.Empty;
        }

        public void Eliminar(int id)
        {
            repository.Eliminar(id);
        }
    }
}
