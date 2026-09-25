# Pedidos SQL Server

Proyecto de prueba en evolucion.

Scripts SQL para crear una base de datos de pedidos con clientes, productos, pedidos, items y auditoria.

## Orden sugerido de ejecucion

1. `01_CreateDatabase.sql`
2. `02_CreateTables.sql`
3. `03_SeedData.sql`
4. `04_StoredProcedures.sql`
5. `05_Triggers.sql`
6. `06_TestQueries.sql`

## Incluye

- Tablas con claves primarias y foraneas
- Restricciones `CHECK`
- Datos iniciales
- Procedimientos almacenados con transacciones
- Triggers de auditoria y recalculo
- Consultas de prueba y reportes

## Relacion con C#

Estos scripts pueden usarse como base para conectar aplicaciones WinForms, MVC o Web API usando ADO.NET, Entity Framework o Dapper.
