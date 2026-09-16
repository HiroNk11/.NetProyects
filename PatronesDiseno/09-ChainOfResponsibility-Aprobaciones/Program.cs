using System;
using System.Collections.Generic;
using ChainOfResponsibilityAprobaciones.Approvers;
using ChainOfResponsibilityAprobaciones.Domain;

namespace ChainOfResponsibilityAprobaciones
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Aprobador supervisor = new Supervisor();
            supervisor
                .DefinirSiguiente(new Gerente())
                .DefinirSiguiente(new Director());

            List<SolicitudGasto> solicitudes = new List<SolicitudGasto>
            {
                new SolicitudGasto("Ana", "Mouse y teclado", 38000),
                new SolicitudGasto("Bruno", "Notebook para desarrollo", 860000),
                new SolicitudGasto("Carla", "Renovacion completa de oficina", 1500000)
            };

            Console.WriteLine("=== Chain of Responsibility - Aprobaciones ===");

            foreach (SolicitudGasto solicitud in solicitudes)
            {
                Console.WriteLine($"{solicitud.Solicitante} solicita ${solicitud.Importe:0.00} por {solicitud.Concepto}.");
                supervisor.Procesar(solicitud);
                Console.WriteLine();
            }

            Console.WriteLine("Presione una tecla para finalizar...");
            Console.ReadKey();
        }
    }
}
