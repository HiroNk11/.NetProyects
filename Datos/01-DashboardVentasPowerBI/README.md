# 01 - Dashboard de ventas para Power BI

Proyecto de datos en C# que genera archivos CSV para crear un dashboard comercial en Power BI.

## Funcionalidades

- Generar datos de clientes
- Generar catalogo de productos
- Generar ventas y detalle de ventas
- Exportar archivos CSV
- Incluir script SQL Server para crear el modelo relacional
- Proponer medidas DAX para Power BI

## Conceptos practicados

- Generacion de datos con C#
- Exportacion a CSV
- Modelo estrella simple
- Tablas de hechos y dimensiones
- Relaciones entre tablas
- Consultas SQL de analisis
- Medidas DAX
- Preparacion de datos para Power BI

## Archivos incluidos

```text
data/
  clientes.csv
  productos.csv
  ventas.csv
  detalle_ventas.csv
sql/
  crear_base.sql
  consultas_analisis.sql
powerbi/
  medidas_dax.md
```

## Modelo para Power BI

Relaciones recomendadas:

- `Clientes[ClienteId]` 1 a muchos con `Ventas[ClienteId]`
- `Ventas[VentaId]` 1 a muchos con `DetalleVentas[VentaId]`
- `Productos[ProductoId]` 1 a muchos con `DetalleVentas[ProductoId]`

Visualizaciones sugeridas:

- Tarjetas: ventas, ganancia, margen y unidades vendidas
- Grafico de columnas: ventas por categoria
- Grafico de barras: ventas por provincia
- Grafico de lineas: ventas por fecha
- Segmentadores: canal, vendedor, categoria y segmento
- Tabla: productos con ventas, costos y ganancia

## Como ejecutar

Abrir el proyecto en Visual Studio y ejecutar con `Ctrl + F5`.

Tambien se puede compilar desde consola:

```powershell
dotnet run --project Datos\01-DashboardVentasPowerBI\DashboardVentasPowerBI.csproj
```

Los CSV se generan en la carpeta `bin/Debug/net48/data`.

## Consigna

Crear un dataset comercial que pueda analizarse en Power BI, identificando ventas, costos, ganancia, margen y rendimiento por categoria, provincia, canal y vendedor.

## Mejora opcional

Agregar una tabla calendario en Power BI y crear medidas de ventas acumuladas, ventas del mes anterior y variacion porcentual mensual.
