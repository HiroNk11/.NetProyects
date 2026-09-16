# 03 - Procesador Pagos SOLID

Aplicacion de consola que procesa pagos usando interfaces y servicios pequenos.

## Conceptos aplicados

- `DIP`: el servicio de checkout depende de interfaces.
- `OCP`: se pueden agregar nuevos metodos de pago sin modificar el checkout.
- `SRP`: cada clase tiene una responsabilidad concreta.

## Metodos incluidos

- Tarjeta de credito
- Transferencia bancaria
- Billetera virtual
