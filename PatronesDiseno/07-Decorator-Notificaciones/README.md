# 07 - Decorator Notificaciones

Ejemplo del patron Decorator aplicado a un sistema de notificaciones.

## Idea

Se parte de una notificacion simple por email y luego se agregan comportamientos sin modificar la clase original.

## Decoradores incluidos

- `SmsNotificadorDecorator`
- `AuditoriaNotificadorDecorator`
- `PrioridadNotificadorDecorator`

## Buenas practicas aplicadas

- Extender comportamiento por composicion.
- Evitar condicionales grandes por canal de envio.
- Mantener clases pequenas y enfocadas.
