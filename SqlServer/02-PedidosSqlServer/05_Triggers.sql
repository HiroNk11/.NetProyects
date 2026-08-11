USE PedidosDb;
GO

CREATE OR ALTER TRIGGER dbo.trg_Productos_AuditarStock
ON dbo.Productos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.StockAuditoria (ProductoId, StockAnterior, StockNuevo, Diferencia, Motivo)
    SELECT
        i.ProductoId,
        d.Stock,
        i.Stock,
        i.Stock - d.Stock,
        CASE
            WHEN i.Stock < d.Stock THEN 'Salida de stock'
            WHEN i.Stock > d.Stock THEN 'Ingreso o devolucion de stock'
            ELSE 'Sin cambio de stock'
        END
    FROM inserted i
    INNER JOIN deleted d ON d.ProductoId = i.ProductoId
    WHERE i.Stock <> d.Stock;
END;
GO

CREATE OR ALTER TRIGGER dbo.trg_PedidoItems_RecalcularTotal
ON dbo.PedidoItems
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH PedidosAfectados AS
    (
        SELECT PedidoId FROM inserted
        UNION
        SELECT PedidoId FROM deleted
    )
    UPDATE p
    SET Total = ISNULL(t.Total, 0)
    FROM dbo.Pedidos p
    INNER JOIN PedidosAfectados pa ON pa.PedidoId = p.PedidoId
    OUTER APPLY
    (
        SELECT SUM(Subtotal) AS Total
        FROM dbo.PedidoItems i
        WHERE i.PedidoId = p.PedidoId
    ) t;
END;
GO
