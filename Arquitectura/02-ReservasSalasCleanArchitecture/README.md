# Reservas Salas Clean Architecture

Proyecto de prueba en evolucion.

Aplicacion de consola para reservar salas de reunion evitando superposiciones de horarios.

## Capas

- `Domain`: entidad `Reserva` y validaciones de fecha.
- `Application`: servicio de reservas y contrato del repositorio.
- `Infrastructure`: repositorio en memoria.
- `Presentation`: flujo de consola.

## Funcionalidades

- Crear reservas.
- Validar horarios.
- Evitar reservas superpuestas en la misma sala.
- Listar reservas ordenadas por fecha.

## Buenas practicas aplicadas

- Reglas de negocio separadas de la consola.
- Dependencia contra interfaces.
- Validaciones dentro del dominio y del servicio de aplicacion.
