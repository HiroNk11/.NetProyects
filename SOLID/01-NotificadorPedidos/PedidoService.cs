using System;

namespace NotificadorPedidos
{
    internal class PedidoService
    {
        private readonly INotificador _notificador;

        public PedidoService(INotificador notificador)
        {
            _notificador = notificador;
        }

        public void Confirmar(Pedido pedido)
        {
            string mensaje = $"Pedido #{pedido.Numero} confirmado por ${pedido.Total:0.00}.";

            Console.WriteLine($"Cliente: {pedido.Cliente}");
            Console.WriteLine("Estado: confirmado");
            _notificador.Enviar(pedido.Contacto, mensaje);
        }
    }
}
