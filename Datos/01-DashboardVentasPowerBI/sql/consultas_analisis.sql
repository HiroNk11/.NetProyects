SELECT
    p.Categoria,
    SUM(d.Cantidad * d.PrecioUnitario) AS Ventas,
    SUM(d.Cantidad * d.CostoUnitario) AS Costos,
    SUM(d.Cantidad * (d.PrecioUnitario - d.CostoUnitario)) AS Ganancia
FROM DetalleVentas d
INNER JOIN Productos p ON p.ProductoId = d.ProductoId
GROUP BY p.Categoria
ORDER BY Ventas DESC;

SELECT
    v.Canal,
    COUNT(DISTINCT v.VentaId) AS CantidadVentas,
    SUM(d.Cantidad * d.PrecioUnitario) AS Ventas
FROM Ventas v
INNER JOIN DetalleVentas d ON d.VentaId = v.VentaId
GROUP BY v.Canal
ORDER BY Ventas DESC;

SELECT
    c.Provincia,
    SUM(d.Cantidad * d.PrecioUnitario) AS Ventas
FROM Ventas v
INNER JOIN Clientes c ON c.ClienteId = v.ClienteId
INNER JOIN DetalleVentas d ON d.VentaId = v.VentaId
GROUP BY c.Provincia
ORDER BY Ventas DESC;
