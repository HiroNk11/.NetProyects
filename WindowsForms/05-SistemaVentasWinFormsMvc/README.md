# 05 - Sistema de ventas WinForms MVC

Aplicacion Windows Forms para cargar clientes, productos y registrar ventas.

## Funcionalidades

- Alta rapida de clientes
- Alta rapida de productos
- Venta con varios items
- Validacion de stock
- Descuento de stock al vender
- Listado de ventas
- Total de venta actual
- Total vendido general

## Buenas practicas aplicadas

- Separacion en `Models`, `Repositories`, `Services` y `Controllers`
- Repositorios con interfaces
- Servicio con reglas de negocio
- Formulario enfocado en la interaccion visual
- Preparado para reemplazar repositorios en memoria por Entity Framework

## Mejora opcional

Agregar un proyecto de datos con Entity Framework y persistir clientes, productos y ventas en SQL Server.
