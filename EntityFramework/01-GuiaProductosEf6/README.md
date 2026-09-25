# Guia Productos EF6

Guia para convertir el proyecto `WindowsForms/06-ProductosWinFormsEfReady` a una version con Entity Framework 6 y SQL Server.

## Objetivo

Reemplazar `InMemoryProductoRepository` por un repositorio que use `DbContext`.

## Plan de integracion pendiente

1. Instalar el paquete NuGet `EntityFramework`.
2. Crear una cadena de conexion en `App.config`.
3. Crear `ProductoDbContext`.
4. Crear `EfProductoRepository`.
5. Cambiar la inyeccion en `MainForm`.

## Archivos de referencia

- `ProductoDbContext.example.cs.txt`
- `EfProductoRepository.example.cs.txt`

Los archivos estan como `.txt` para no romper la compilacion si Entity Framework todavia no esta instalado.
