using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AgendaContactosCsv
{
    internal static class ArchivoContactos
    {
        private const string RutaArchivo = "contactos.csv";

        public static List<Contacto> Cargar()
        {
            List<Contacto> contactos = new List<Contacto>();

            if (!File.Exists(RutaArchivo))
            {
                return contactos;
            }

            string[] lineas = File.ReadAllLines(RutaArchivo);

            foreach (string linea in lineas)
            {
                string[] campos = linea.Split(';');

                if (campos.Length != 4 || !int.TryParse(campos[0], out int id))
                {
                    continue;
                }

                contactos.Add(new Contacto
                {
                    Id = id,
                    Nombre = campos[1],
                    Telefono = campos[2],
                    Email = campos[3]
                });
            }

            return contactos;
        }

        public static void Guardar(List<Contacto> contactos)
        {
            List<string> lineas = contactos
                .Select(contacto => $"{contacto.Id};{LimpiarCampo(contacto.Nombre)};{LimpiarCampo(contacto.Telefono)};{LimpiarCampo(contacto.Email)}")
                .ToList();

            File.WriteAllLines(RutaArchivo, lineas);
        }

        private static string LimpiarCampo(string valor)
        {
            return valor.Replace(";", ",").Trim();
        }
    }
}
