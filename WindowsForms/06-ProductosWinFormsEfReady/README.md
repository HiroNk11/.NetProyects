# Productos WinForms EF Ready

Proyecto de prueba en evolucion.

Aplicacion Windows Forms para CRUD de productos, preparada para cambiar el repositorio en memoria por Entity Framework.

## Funcionalidades

- Crear productos
- Editar productos
- Eliminar productos
- Marcar productos activos o inactivos
- Validar nombre y precio

## Buenas practicas aplicadas

- Interfaz `IProductoRepository`
- Implementacion `InMemoryProductoRepository`
- Servicio `ProductoService`
- Modelo `Producto`
- Formulario separado de la persistencia

## Paso hacia Entity Framework

El formulario depende de `ProductoService`, y el servicio depende de `IProductoRepository`. Para usar Entity Framework, se crea otra implementacion del repositorio y se cambia la inyeccion en `MainForm`.
