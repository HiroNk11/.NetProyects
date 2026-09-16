using System;
using System.Collections.Generic;
using System.IO;
using SerializacionJsonXml.Domain;
using SerializacionJsonXml.Persistence;

namespace SerializacionJsonXml
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Producto> productos = new List<Producto>
            {
                new Producto(1, "Notebook", 1200000),
                new Producto(2, "Monitor", 260000),
                new Producto(3, "Teclado", 95000)
            };

            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            string jsonPath = Path.Combine(dataPath, "productos.json");
            string xmlPath = Path.Combine(dataPath, "productos.xml");

            JsonProductoRepository jsonRepository = new JsonProductoRepository(jsonPath);
            XmlProductoRepository xmlRepository = new XmlProductoRepository(xmlPath);

            jsonRepository.Guardar(productos);
            xmlRepository.Guardar(productos);

            Console.WriteLine("=== Herramientas - Serializacion JSON XML ===");
            Console.WriteLine($"JSON generado: {jsonPath}");
            Console.WriteLine($"XML generado: {xmlPath}");
            Console.WriteLine();
            Console.WriteLine("Productos leidos desde JSON:");

            foreach (Producto producto in jsonRepository.Leer())
            {
                Console.WriteLine($"{producto.Id}. {producto.Nombre} - ${producto.Precio:0.00}");
            }

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
