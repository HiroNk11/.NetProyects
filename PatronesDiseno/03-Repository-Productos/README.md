# Repository - Productos

Proyecto de prueba en evolucion.

Aplicacion de consola en C# que usa el patron Repository para separar la logica de negocio del almacenamiento de productos.

## Funcionalidades

- Listar productos
- Consultar productos por Id
- Registrar ventas
- Validar stock disponible
- Descontar stock despues de una venta
- Usar un repositorio en memoria

## Implementacion

- Patron Repository
- Interfaces
- Inyeccion de dependencias por constructor
- Separacion entre servicio y almacenamiento
- Reglas de negocio
- Validaciones de stock

## Evolucion del proyecto

Propuesta pendiente para una proxima version:

Crear una implementacion `ProductoRepositorySqlServer` usando ADO.NET sin modificar `ProductoService`.
