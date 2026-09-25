# Adapter Exportacion

Proyecto de prueba en evolucion.

Prototipo con el patron Adapter aplicado a exportacion de reportes.

## Diseno

La aplicacion trabaja con una interfaz propia (`IExportadorReporte`), aunque por dentro use clases externas con metodos diferentes.

## Casos incluidos

- Exportacion a CSV con una clase interna.
- Exportacion a JSON usando un servicio externo simulado.
- Exportacion a PDF usando un servicio externo simulado.

## Buenas practicas aplicadas

- La aplicacion no depende directamente de librerias externas.
- Cada adaptador traduce el contrato externo al contrato interno.
- Se pueden agregar nuevos formatos sin modificar el flujo principal.
