using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using SerializacionJsonXml.Domain;

namespace SerializacionJsonXml.Persistence
{
    internal class XmlProductoRepository
    {
        private readonly string _filePath;

        public XmlProductoRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Guardar(List<Producto> productos)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            using (FileStream stream = File.Create(_filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Producto>));
                serializer.Serialize(stream, productos);
            }
        }

        public List<Producto> Leer()
        {
            using (FileStream stream = File.OpenRead(_filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Producto>));
                return (List<Producto>)serializer.Deserialize(stream);
            }
        }
    }
}
