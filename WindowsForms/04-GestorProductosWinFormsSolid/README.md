# Gestor de productos WinForms SOLID

Proyecto de prueba en evolucion.

Aplicacion Windows Forms para administrar productos con separacion por capas.

## Funcionalidades

- Alta de productos
- Edicion de productos
- Eliminacion de productos
- Busqueda por nombre o categoria
- Calculo de valor total de stock

## Buenas practicas aplicadas

- Modelo `Producto`
- Interfaz `IProductoRepository`
- Repositorio en memoria intercambiable
- Servicio `ProductoService`
- Validaciones fuera del formulario
- Formulario enfocado en UI

## Relacion con Entity Framework

El repositorio en memoria puede reemplazarse por un repositorio con Entity Framework sin cambiar el formulario.
