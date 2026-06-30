# 03 - Repository - Productos

Aplicacion de consola en C# que usa el patron Repository para separar la logica de negocio del almacenamiento de productos.

## Funcionalidades

- Listar productos
- Consultar productos por Id
- Registrar ventas
- Validar stock disponible
- Descontar stock despues de una venta
- Usar un repositorio en memoria

## Conceptos practicados

- Patron Repository
- Interfaces
- Inyeccion de dependencias por constructor
- Separacion entre servicio y almacenamiento
- Reglas de negocio
- Validaciones de stock

## Consigna

Crear un sistema de productos donde el servicio de negocio dependa de una interfaz `IProductoRepository` y no de una lista concreta.

## Mejora opcional

Crear una implementacion `ProductoRepositorySqlServer` usando ADO.NET sin modificar `ProductoService`.
