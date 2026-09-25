# Biblioteca API MVC EF6

Proyecto de prueba en evolucion.

API REST de biblioteca usando ASP.NET Web API 2, .NET Framework 4.8, Entity Framework 6 y SQL Server LocalDB.

## Entidades

- `Socio`
- `Libro`
- `Prestamo`

## Endpoints

| Metodo | Ruta | Descripcion |
| --- | --- | --- |
| GET | `/api/socios` | Lista socios |
| POST | `/api/socios` | Crea un socio |
| GET | `/api/libros` | Lista libros |
| POST | `/api/libros` | Crea un libro |
| GET | `/api/prestamos` | Lista prestamos |
| POST | `/api/prestamos` | Registra un prestamo |
| POST | `/api/prestamos/{id}/devolver` | Devuelve un libro |

## Ejemplo crear socio

```json
{
  "nombre": "Ana Garcia",
  "email": "ana@email.com"
}
```

## Ejemplo crear libro

```json
{
  "titulo": "Clean Code",
  "autor": "Robert C. Martin",
  "anioPublicacion": 2008
}
```

## Ejemplo crear prestamo

```json
{
  "socioId": 1,
  "libroId": 1,
  "diasPrestamo": 14
}
```

## Buenas practicas aplicadas

- DTOs para entrada y salida
- Repositorios con interfaces
- Servicios con reglas de negocio
- Validacion de socio activo
- Validacion de libro disponible
- Cambio de disponibilidad al prestar y devolver
- Entity Framework 6 con relaciones

## Como ejecutar

1. Abrir el proyecto en Visual Studio.
2. Restaurar paquetes NuGet.
3. Ejecutar con IIS Express.
4. Probar endpoints con Postman o Thunder Client.
