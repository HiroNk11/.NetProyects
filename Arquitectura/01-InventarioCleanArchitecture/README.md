# 01 - Inventario Clean Architecture

Aplicacion de consola para administrar productos de inventario usando una estructura cercana a Clean Architecture.

## Capas

- `Domain`: entidades y reglas propias del negocio.
- `Application`: casos de uso y contratos.
- `Infrastructure`: implementaciones concretas de almacenamiento.
- `Presentation`: menu de consola.

## Funcionalidades

- Listar productos
- Agregar productos
- Registrar entradas de stock
- Registrar salidas de stock
- Ver productos con stock bajo
- Calcular valor total del inventario

## Buenas practicas aplicadas

- La capa de aplicacion depende de interfaces, no de clases concretas.
- Las reglas basicas de stock viven dentro de la entidad `Producto`.
- El menu de consola solo coordina entrada y salida de datos.
- El repositorio en memoria se puede reemplazar por SQL Server o Entity Framework.

## Como ejecutar

Abrir el proyecto en Visual Studio y ejecutar con `Ctrl + F5`.
