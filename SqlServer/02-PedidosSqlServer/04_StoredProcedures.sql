USE PedidosDb;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Cliente_Crear
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(120),
    @Telefono NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NULLIF(LTRIM(RTRIM(@Nombre)), '') IS NULL
        THROW 50001, 'El nombre es obligatorio.', 1;

    IF NULLIF(LTRIM(RTRIM(@Email)), '') IS NULL
        THROW 50002, 'El email es obligatorio.', 1;

    INSERT INTO dbo.Clientes (Nombre, Email, Telefono)
    VALUES (LTRIM(RTRIM(@Nombre)), LTRIM(RTRIM(@Email)), @Telefono);

    SELECT SCOPE_IDENTITY() AS ClienteId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Producto_Crear
    @Nombre NVARCHAR(100),
    @Precio DECIMAL(18,2),
    @Stock INT,
    @StockMinimo INT = 5
AS
BEGIN
    SET NOCOUNT ON;

    IF NULLIF(LTRIM(RTRIM(@Nombre)), '') IS NULL
        THROW 50003, 'El nombre del producto es obligatorio.', 1;

    IF @Precio <= 0
        THROW 50004, 'El precio debe ser mayor a cero.', 1;

    IF @Stock < 0
        THROW 50005, 'El stock no puede ser negativo.', 1;

    INSERT INTO dbo.Productos (Nombre, Precio, Stock, StockMinimo)
    VALUES (LTRIM(RTRIM(@Nombre)), @Precio, @Stock, @StockMinimo);

    SELECT SCOPE_IDENTITY() AS ProductoId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pedido_Crear
    @ClienteId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Clientes WHERE ClienteId = @ClienteId AND Activo = 1)
        THROW 50006, 'Cliente inexistente o inactivo.', 1;

    INSERT INTO dbo.Pedidos (ClienteId)
    VALUES (@ClienteId);

    SELECT SCOPE_IDENTITY() AS PedidoId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pedido_AgregarItem
    @PedidoId INT,
    @ProductoId INT,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Precio DECIMAL(18,2);
    DECLARE @StockActual INT;
    DECLARE @Estado NVARCHAR(20);

    SELECT @Estado = Estado
    FROM dbo.Pedidos
    WHERE PedidoId = @PedidoId;

    IF @Estado IS NULL
        THROW 50007, 'Pedido inexistente.', 1;

    IF @Estado <> 'Pendiente'
        THROW 50008, 'Solo se pueden modificar pedidos pendientes.', 1;

    IF @Cantidad <= 0
        THROW 50009, 'La cantidad debe ser mayor a cero.', 1;

    BEGIN TRANSACTION;

    SELECT
        @Precio = Precio,
        @StockActual = Stock
    FROM dbo.Productos WITH (UPDLOCK, ROWLOCK)
    WHERE ProductoId = @ProductoId
      AND Activo = 1;

    IF @Precio IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50010, 'Producto inexistente o inactivo.', 1;
    END;

    IF @StockActual < @Cantidad
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50011, 'Stock insuficiente.', 1;
    END;

    INSERT INTO dbo.PedidoItems (PedidoId, ProductoId, Cantidad, PrecioUnitario)
    VALUES (@PedidoId, @ProductoId, @Cantidad, @Precio);

    UPDATE dbo.Productos
    SET Stock = Stock - @Cantidad
    WHERE ProductoId = @ProductoId;

    UPDATE dbo.Pedidos
    SET Total = (
        SELECT ISNULL(SUM(Subtotal), 0)
        FROM dbo.PedidoItems
        WHERE PedidoId = @PedidoId
    )
    WHERE PedidoId = @PedidoId;

    COMMIT TRANSACTION;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pedido_Facturar
    @PedidoId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Pedidos WHERE PedidoId = @PedidoId AND Estado = 'Pendiente')
        THROW 50012, 'Pedido inexistente o no pendiente.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.PedidoItems WHERE PedidoId = @PedidoId)
        THROW 50013, 'No se puede facturar un pedido sin items.', 1;

    UPDATE dbo.Pedidos
    SET Estado = 'Facturado'
    WHERE PedidoId = @PedidoId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Pedido_Cancelar
    @PedidoId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Pedidos WHERE PedidoId = @PedidoId AND Estado <> 'Cancelado')
        THROW 50014, 'Pedido inexistente o ya cancelado.', 1;

    BEGIN TRANSACTION;

    UPDATE p
    SET p.Stock = p.Stock + i.Cantidad
    FROM dbo.Productos p
    INNER JOIN dbo.PedidoItems i ON i.ProductoId = p.ProductoId
    WHERE i.PedidoId = @PedidoId;

    UPDATE dbo.Pedidos
    SET Estado = 'Cancelado'
    WHERE PedidoId = @PedidoId;

    COMMIT TRANSACTION;
END;
GO
