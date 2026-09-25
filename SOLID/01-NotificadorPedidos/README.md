# Notificador de pedidos

Proyecto de prueba en evolucion.

Aplicacion de consola en C# que confirma un pedido y envia una notificacion por distintos canales.

## Funcionalidades

- Confirmar un pedido de ejemplo
- Elegir canal de notificacion
- Enviar notificacion por Email
- Enviar notificacion por SMS
- Enviar notificacion por WhatsApp
- Cambiar el canal sin modificar la clase `PedidoService`

## Implementacion

- Principio de responsabilidad unica
- Principio abierto/cerrado
- Principio de inversion de dependencias
- Interfaces
- Inyeccion de dependencias por constructor
- Patron Factory simple
- Separacion entre reglas de negocio y detalles de comunicacion

## Evolucion del proyecto

Propuesta pendiente para una proxima version:

Agregar un nuevo canal, por ejemplo `PushNotificador`, sin modificar `PedidoService`.
