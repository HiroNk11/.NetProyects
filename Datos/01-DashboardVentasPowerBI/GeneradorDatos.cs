using System;
using System.Collections.Generic;

namespace DashboardVentasPowerBI
{
    internal class GeneradorDatos
    {
        private readonly Random _random = new Random(2026);

        public List<Cliente> CrearClientes()
        {
            return new List<Cliente>
            {
                new Cliente { Id = 1, Nombre = "Norte Retail", Segmento = "Empresa", Provincia = "Buenos Aires" },
                new Cliente { Id = 2, Nombre = "Patagonia Hogar", Segmento = "Empresa", Provincia = "Rio Negro" },
                new Cliente { Id = 3, Nombre = "Cliente Mostrador", Segmento = "Consumidor final", Provincia = "Cordoba" },
                new Cliente { Id = 4, Nombre = "Tecno Sur", Segmento = "Empresa", Provincia = "Mendoza" },
                new Cliente { Id = 5, Nombre = "Oficina Centro", Segmento = "PyME", Provincia = "Santa Fe" },
                new Cliente { Id = 6, Nombre = "Distribuidora Litoral", Segmento = "Mayorista", Provincia = "Entre Rios" },
                new Cliente { Id = 7, Nombre = "Compras Online", Segmento = "Consumidor final", Provincia = "Buenos Aires" },
                new Cliente { Id = 8, Nombre = "Servicios Andinos", Segmento = "PyME", Provincia = "Neuquen" }
            };
        }

        public List<Producto> CrearProductos()
        {
            return new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Notebook Pro 14", Categoria = "Tecnologia", Costo = 720000, Precio = 980000 },
                new Producto { Id = 2, Nombre = "Monitor 24", Categoria = "Tecnologia", Costo = 145000, Precio = 210000 },
                new Producto { Id = 3, Nombre = "Silla ergonomica", Categoria = "Oficina", Costo = 98000, Precio = 155000 },
                new Producto { Id = 4, Nombre = "Escritorio compacto", Categoria = "Oficina", Costo = 87000, Precio = 132000 },
                new Producto { Id = 5, Nombre = "Auriculares bluetooth", Categoria = "Accesorios", Costo = 27000, Precio = 48000 },
                new Producto { Id = 6, Nombre = "Teclado mecanico", Categoria = "Accesorios", Costo = 42000, Precio = 76000 },
                new Producto { Id = 7, Nombre = "Impresora laser", Categoria = "Tecnologia", Costo = 160000, Precio = 235000 },
                new Producto { Id = 8, Nombre = "Lampara escritorio", Categoria = "Oficina", Costo = 18000, Precio = 35000 }
            };
        }

        public List<Venta> CrearVentas(List<Cliente> clientes)
        {
            string[] canales = { "Tienda", "Online", "Telefono" };
            string[] vendedores = { "Ana", "Bruno", "Carla", "Diego" };
            List<Venta> ventas = new List<Venta>();

            for (int i = 1; i <= 120; i++)
            {
                ventas.Add(new Venta
                {
                    Id = i,
                    Fecha = new DateTime(2026, 1, 1).AddDays(_random.Next(0, 180)),
                    ClienteId = clientes[_random.Next(clientes.Count)].Id,
                    Canal = canales[_random.Next(canales.Length)],
                    Vendedor = vendedores[_random.Next(vendedores.Length)]
                });
            }

            return ventas;
        }

        public List<DetalleVenta> CrearDetalles(List<Venta> ventas, List<Producto> productos)
        {
            List<DetalleVenta> detalles = new List<DetalleVenta>();

            foreach (Venta venta in ventas)
            {
                int cantidadLineas = _random.Next(1, 4);
                List<Producto> productosDisponibles = new List<Producto>(productos);

                for (int i = 0; i < cantidadLineas; i++)
                {
                    int indiceProducto = _random.Next(productosDisponibles.Count);
                    Producto producto = productosDisponibles[indiceProducto];
                    productosDisponibles.RemoveAt(indiceProducto);
                    decimal variacionPrecio = 1 + _random.Next(-8, 9) / 100m;

                    detalles.Add(new DetalleVenta
                    {
                        VentaId = venta.Id,
                        ProductoId = producto.Id,
                        Cantidad = _random.Next(1, 6),
                        PrecioUnitario = Math.Round(producto.Precio * variacionPrecio, 2),
                        CostoUnitario = producto.Costo
                    });
                }
            }

            return detalles;
        }
    }
}
