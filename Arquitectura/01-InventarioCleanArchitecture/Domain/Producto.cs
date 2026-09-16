using System;

namespace InventarioCleanArchitecture.Domain
{
    internal class Producto
    {
        public Producto(int id, string nombre, decimal precio, int stock, int stockMinimo)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El id debe ser mayor a cero.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));
            }

            if (precio <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor a cero.", nameof(precio));
            }

            if (stock < 0)
            {
                throw new ArgumentException("El stock no puede ser negativo.", nameof(stock));
            }

            if (stockMinimo < 0)
            {
                throw new ArgumentException("El stock minimo no puede ser negativo.", nameof(stockMinimo));
            }

            Id = id;
            Nombre = nombre.Trim();
            Precio = precio;
            Stock = stock;
            StockMinimo = stockMinimo;
        }

        public int Id { get; }

        public string Nombre { get; }

        public decimal Precio { get; private set; }

        public int Stock { get; private set; }

        public int StockMinimo { get; }

        public void ActualizarPrecio(decimal nuevoPrecio)
        {
            if (nuevoPrecio <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor a cero.", nameof(nuevoPrecio));
            }

            Precio = nuevoPrecio;
        }

        public void AgregarStock(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(cantidad));
            }

            Stock += cantidad;
        }

        public bool DescontarStock(int cantidad)
        {
            if (cantidad <= 0 || cantidad > Stock)
            {
                return false;
            }

            Stock -= cantidad;
            return true;
        }

        public bool TieneStockBajo()
        {
            return Stock <= StockMinimo;
        }

        public decimal CalcularValorStock()
        {
            return Precio * Stock;
        }
    }
}
