# Observer Alertas Stock

Proyecto de prueba en evolucion.

Aplicacion de consola que muestra el patron Observer con alertas cuando un producto llega a stock bajo.

## Diseno

El producto no conoce los detalles de cada alerta. Solo notifica a los observadores suscriptos cuando cambia el stock.

## Observadores incluidos

- `ConsoleStockObserver`: muestra una alerta simple en consola.
- `EmailStockObserver`: simula el envio de un email al equipo de compras.
- `DashboardStockObserver`: simula una actualizacion para un tablero interno.

## Buenas practicas aplicadas

- Bajo acoplamiento entre la entidad y los canales de notificacion.
- Posibilidad de agregar nuevos observadores sin modificar `Producto`.
- Uso del principio Open/Closed.

## Como ejecutar

Abrir el proyecto en Visual Studio y ejecutar con `Ctrl + F5`.
