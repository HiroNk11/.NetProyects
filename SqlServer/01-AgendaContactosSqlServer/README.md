# Agenda de contactos con SQL Server

Proyecto de prueba en evolucion.

Aplicacion de consola en C# que permite guardar contactos en una base de datos SQL Server usando ADO.NET.

## Funcionalidades

- Crear la tabla `Contactos` si no existe
- Listar contactos guardados
- Agregar contactos
- Eliminar contactos por Id
- Usar comandos SQL parametrizados
- Manejar errores de conexion

## Implementacion

- Conexion a SQL Server
- ADO.NET
- `SqlConnection`
- `SqlCommand`
- `SqlDataReader`
- Parametros SQL
- Patron Repository simple
- Separacion entre modelo, acceso a datos y menu

## Base de datos

El proyecto usa LocalDB por defecto:

```text
Server=(localdb)\MSSQLLocalDB;Database=AgendaContactosDb;Integrated Security=True;
```

Si se usa otra instancia de SQL Server, modificar la constante `ConnectionString` en `Program.cs`.

## Evolucion del proyecto

Propuesta pendiente para una proxima version:

Agregar la opcion de editar contactos o buscar por nombre.
