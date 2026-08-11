using AgendaContactosWinFormsMvc.Models;

namespace AgendaContactosWinFormsMvc.Services
{
    internal class ContactoValidator
    {
        public string Validar(Contacto contacto)
        {
            if (string.IsNullOrWhiteSpace(contacto.Nombre))
            {
                return "El nombre es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(contacto.Telefono))
            {
                return "El telefono es obligatorio.";
            }

            if (string.IsNullOrWhiteSpace(contacto.Email) || !contacto.Email.Contains("@") || !contacto.Email.Contains("."))
            {
                return "Ingrese un email valido.";
            }

            return string.Empty;
        }
    }
}
