using System;

namespace NotificadorPedidos
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=== Notificador de pedidos ===");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. SMS");
            Console.WriteLine("3. WhatsApp");
            Console.Write("Seleccione el canal de notificacion: ");

            string opcion = Console.ReadLine();
            INotificador notificador = NotificadorFactory.Crear(opcion);
            PedidoService pedidoService = new PedidoService(notificador);

            Pedido pedido = new Pedido(1001, "Cliente Demo", "cliente@demo.com", 15999.99m);

            Console.WriteLine();
            pedidoService.Confirmar(pedido);

            Console.WriteLine();
            Console.WriteLine("Presione una tecla para salir...");
            Console.ReadKey();
        }
    }
}
