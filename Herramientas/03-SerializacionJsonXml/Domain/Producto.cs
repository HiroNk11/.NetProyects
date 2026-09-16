using System.Runtime.Serialization;

namespace SerializacionJsonXml.Domain
{
    [DataContract]
    public class Producto
    {
        public Producto()
        {
        }

        public Producto(int id, string nombre, decimal precio)
        {
            Id = id;
            Nombre = nombre;
            Precio = precio;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public decimal Precio { get; set; }
    }
}
