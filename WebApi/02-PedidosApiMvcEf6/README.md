# 02 - Pedidos API MVC EF6

API REST de pedidos usando ASP.NET Web API 2, .NET Framework 4.8, MVC/Web API, Repository, Services y Entity Framework 6.

## Entidades

- `Cliente`
- `Producto`
- `Pedido`
- `ItemPedido`

## Endpoints

| Metodo | Ruta | Descripcion |
| --- | --- | --- |
| GET | `/api/clientes` | Lista clientes |
| POST | `/api/clientes` | Crea un cliente |
| GET | `/api/productos` | Lista productos |
| GET | `/api/productos/{id}` | Obtiene un producto |
| POST | `/api/productos` | Crea un producto |
| GET | `/api/pedidos` | Lista pedidos |
| GET | `/api/pedidos/{id}` | Obtiene un pedido con items |
| POST | `/api/pedidos` | Crea un pedido |
| POST | `/api/pedidos/{id}/cancelar` | Cancela un pedido y devuelve stock |

## Ejemplo crear cliente

```json
{
  "nombre": "Nicolas Gomez",
  "email": "nico@email.com"
}
```

## Ejemplo crear producto

```json
{
  "nombre": "Teclado mecanico",
  "precio": 45000,
  "stock": 12
}
```

## Ejemplo crear pedido

```json
{
  "clienteId": 1,
  "items": [
    {
      "productoId": 1,
      "cantidad": 2
    }
  ]
}
```

## Buenas practicas aplicadas

- Endpoints REST separados por recurso
- DTOs para entrada de datos
- Servicios con reglas de negocio
- Repositorios con interfaces
- Entity Framework 6 con relaciones
- Validacion de stock antes de crear pedidos
- Devolucion de stock al cancelar
- `Include` para cargar relaciones

## Como ejecutar

1. Abrir el proyecto en Visual Studio.
2. Restaurar paquetes NuGet.
3. Ejecutar con IIS Express.
4. Probar endpoints con Postman o Thunder Client.
