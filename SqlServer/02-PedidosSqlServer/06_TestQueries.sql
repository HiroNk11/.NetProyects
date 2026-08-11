USE PedidosDb;
GO

EXEC dbo.sp_Cliente_Crear
    @Nombre = 'Empresa Demo SRL',
    @Email = 'compras@empresademo.com',
    @Telefono = '1155559999';
GO

EXEC dbo.sp_Producto_Crear
    @Nombre = 'Webcam HD',
    @Precio = 38000.00,
    @Stock = 15,
    @StockMinimo = 4;
GO

DECLARE @NuevoPedidoId INT;

EXEC dbo.sp_Pedido_Crear @ClienteId = 1;

SELECT @NuevoPedidoId = MAX(PedidoId)
FROM dbo.Pedidos
WHERE ClienteId = 1;

EXEC dbo.sp_Pedido_AgregarItem
    @PedidoId = @NuevoPedidoId,
    @ProductoId = 1,
    @Cantidad = 2;

EXEC dbo.sp_Pedido_AgregarItem
    @PedidoId = @NuevoPedidoId,
    @ProductoId = 2,
    @Cantidad = 1;

EXEC dbo.sp_Pedido_Facturar @PedidoId = @NuevoPedidoId;
GO

SELECT
    p.PedidoId,
    c.Nombre AS Cliente,
    p.Fecha,
    p.Estado,
    p.Total
FROM dbo.Pedidos p
INNER JOIN dbo.Clientes c ON c.ClienteId = p.ClienteId
ORDER BY p.Fecha DESC;
GO

SELECT
    c.Nombre AS Cliente,
    COUNT(p.PedidoId) AS CantidadPedidos,
    SUM(p.Total) AS TotalVendido
FROM dbo.Clientes c
INNER JOIN dbo.Pedidos p ON p.ClienteId = c.ClienteId
WHERE p.Estado = 'Facturado'
GROUP BY c.Nombre
ORDER BY TotalVendido DESC;
GO

SELECT
    ProductoId,
    Nombre,
    Stock,
    StockMinimo
FROM dbo.Productos
WHERE Stock <= StockMinimo
ORDER BY Stock ASC;
GO

SELECT
    a.StockAuditoriaId,
    p.Nombre AS Producto,
    a.StockAnterior,
    a.StockNuevo,
    a.Diferencia,
    a.Motivo,
    a.Fecha
FROM dbo.StockAuditoria a
INNER JOIN dbo.Productos p ON p.ProductoId = a.ProductoId
ORDER BY a.Fecha DESC;
GO
