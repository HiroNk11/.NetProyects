# Agenda de contactos con CSV

Proyecto de prueba en evolucion.

Aplicacion de consola en C# para administrar contactos y guardarlos en un archivo CSV.

## Funcionalidades

- Cargar contactos desde `contactos.csv`
- Agregar contactos
- Listar contactos
- Buscar contactos por nombre
- Editar contactos
- Eliminar contactos
- Guardar cambios automaticamente
- Validar campos obligatorios
- Validar email de forma simple

## Implementacion

- Manejo de archivos con `File`
- Lectura con `File.ReadAllLines`
- Escritura con `File.WriteAllLines`
- Separacion simple de datos con CSV
- Clases estaticas
- `List<T>`
- LINQ con `Where`, `FirstOrDefault` y `Max`

## Evolucion del proyecto

Propuesta pendiente para una proxima version:

Agregar confirmacion antes de eliminar o exportar los contactos ordenados alfabeticamente.
