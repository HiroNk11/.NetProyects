# Command Operaciones

Proyecto de prueba en evolucion.

Prototipo con el patron Command aplicado a operaciones bancarias con historial y deshacer.

## Diseno

Cada accion se representa como un comando. El invocador ejecuta comandos sin conocer los detalles de la cuenta bancaria.

## Funcionalidades

- Depositar dinero.
- Extraer dinero.
- Consultar saldo.
- Deshacer la ultima operacion valida.

## Buenas practicas aplicadas

- Encapsulacion de acciones.
- Historial de operaciones.
- Separacion entre invocador, comando y receptor.
