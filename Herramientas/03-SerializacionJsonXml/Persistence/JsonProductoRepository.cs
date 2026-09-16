using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using SerializacionJsonXml.Domain;

namespace SerializacionJsonXml.Persistence
{
    internal class JsonProductoRepository
    {
        private readonly string _filePath;

        public JsonProductoRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Guardar(List<Producto> productos)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            using (FileStream stream = File.Create(_filePath))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Producto>));
                serializer.WriteObject(stream, productos);
            }
        }

        public List<Producto> Leer()
        {
            using (FileStream stream = File.OpenRead(_filePath))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Producto>));
                return (List<Producto>)serializer.ReadObject(stream);
            }
        }
    }
}
