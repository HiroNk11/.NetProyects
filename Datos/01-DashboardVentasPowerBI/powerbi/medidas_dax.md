# Medidas DAX sugeridas

Crear estas medidas sobre la tabla `DetalleVentas`.

```DAX
Ventas = SUMX(DetalleVentas, DetalleVentas[Cantidad] * DetalleVentas[PrecioUnitario])
```

```DAX
Costos = SUMX(DetalleVentas, DetalleVentas[Cantidad] * DetalleVentas[CostoUnitario])
```

```DAX
Ganancia = [Ventas] - [Costos]
```

```DAX
Margen % = DIVIDE([Ganancia], [Ventas])
```

```DAX
Unidades Vendidas = SUM(DetalleVentas[Cantidad])
```

```DAX
Ticket Promedio = DIVIDE([Ventas], DISTINCTCOUNT(Ventas[VentaId]))
```
