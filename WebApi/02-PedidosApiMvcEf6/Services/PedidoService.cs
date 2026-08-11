using System;
using System.Collections.Generic;
using System.Linq;
using PedidosApiMvcEf6.Dtos;
using PedidosApiMvcEf6.Models;
using PedidosApiMvcEf6.Repositories;

namespace PedidosApiMvcEf6.Services
{
    public class PedidoService
    {
        private const string EstadoPendiente = "Pendiente";
        private const string EstadoCancelado = "Cancelado";

        private readonly IClienteRepository clienteRepository;
        private readonly IProductoRepository productoRepository;
        private readonly IPedidoRepository pedidoRepository;

        public PedidoService(IClienteRepository clienteRepository, IProductoRepository productoRepository, IPedidoRepository pedidoRepository)
        {
            this.clienteRepository = clienteRepository;
            this.productoRepository = productoRepository;
            this.pedidoRepository = pedidoRepository;
        }

        public List<PedidoResumenDto> ObtenerTodos()
        {
            return pedidoRepository.ObtenerTodos()
                .Select(MapearResumen)
                .ToList();
        }

        public Pedido ObtenerPorId(int id)
        {
            return pedidoRepository.ObtenerPorId(id);
        }

        public string Crear(PedidoCreateDto dto, out Pedido pedido)
        {
            pedido = null;
            Cliente cliente = clienteRepository.ObtenerPorId(dto.ClienteId);

            if (cliente == null)
            {
                return "Cliente no encontrado.";
            }

            if (dto.Items == null || dto.Items.Count == 0)
            {
                return "El pedido debe tener al menos un item.";
            }

            List<ItemPedido> items = new List<ItemPedido>();

            foreach (ItemPedidoCreateDto itemDto in dto.Items)
            {
                Producto producto = productoRepository.ObtenerPorId(itemDto.ProductoId);

                if (producto == null || !producto.Activo)
                {
                    return $"Producto {itemDto.ProductoId} no disponible.";
                }

                if (itemDto.Cantidad <= 0)
                {
                    return "La cantidad debe ser mayor a cero.";
                }

                if (itemDto.Cantidad > producto.Stock)
                {
                    return $"Stock insuficiente para {producto.Nombre}.";
                }

                items.Add(new ItemPedido
                {
                    ProductoId = producto.Id,
                    Producto = producto,
                    Cantidad = itemDto.Cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            foreach (ItemPedido item in items)
            {
                item.Producto.Stock -= item.Cantidad;
                productoRepository.Actualizar(item.Producto);
            }

            pedido = new Pedido
            {
                ClienteId = cliente.Id,
                Cliente = cliente,
                Fecha = DateTime.Now,
                Estado = EstadoPendiente
            };

            foreach (ItemPedido item in items)
            {
                pedido.Items.Add(item);
            }

            pedidoRepository.Agregar(pedido);
            return string.Empty;
        }

        public bool Cancelar(int id)
        {
            Pedido pedido = pedidoRepository.ObtenerPorId(id);

            if (pedido == null || pedido.Estado == EstadoCancelado)
            {
                return false;
            }

            pedido.Estado = EstadoCancelado;

            foreach (ItemPedido item in pedido.Items)
            {
                Producto producto = productoRepository.ObtenerPorId(item.ProductoId);

                if (producto != null)
                {
                    producto.Stock += item.Cantidad;
                    productoRepository.Actualizar(producto);
                }
            }

            pedidoRepository.Actualizar(pedido);
            return true;
        }

        private PedidoResumenDto MapearResumen(Pedido pedido)
        {
            return new PedidoResumenDto
            {
                Id = pedido.Id,
                Fecha = pedido.Fecha,
                Cliente = pedido.Cliente.Nombre,
                Estado = pedido.Estado,
                Total = pedido.Total
            };
        }
    }
}
