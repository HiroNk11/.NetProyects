# 01 - Productos API MVC EF6

API REST de productos usando ASP.NET Web API 2, .NET Framework 4.8, Entity Framework 6 y SQL Server LocalDB.

## Endpoints

| Metodo | Ruta | Descripcion |
| --- | --- | --- |
| GET | `/api/productos` | Lista productos |
| GET | `/api/productos/{id}` | Obtiene un producto por ID |
| POST | `/api/productos` | Crea un producto |
| PUT | `/api/productos/{id}` | Actualiza un producto |
| DELETE | `/api/productos/{id}` | Elimina un producto |

## Ejemplo POST

```json
{
  "nombre": "Mouse Logitech",
  "precio": 15000,
  "stock": 10
}
```

## Ejemplo PUT

```json
{
  "nombre": "Mouse Logitech",
  "precio": 18000,
  "stock": 8,
  "activo": true
}
```

## Buenas practicas aplicadas

- Controlador HTTP separado de reglas de negocio
- DTOs para entrada de datos
- Servicio `ProductoService`
- Repositorio con interfaz `IProductoRepository`
- Implementacion `EfProductoRepository`
- `DbContext` separado en `Data`
- Validaciones en capa de servicio
- Configuracion de rutas en `WebApiConfig`

## Como ejecutar

1. Abrir el proyecto en Visual Studio.
2. Restaurar paquetes NuGet.
3. Ejecutar con IIS Express.
4. Probar endpoints con Postman, Thunder Client o el navegador.

## Notas

Este proyecto usa `PackageReference` para restaurar paquetes desde Visual Studio o `dotnet restore`. La base se crea en LocalDB usando la cadena `ProductosDb` de `Web.config`.
