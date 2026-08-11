USE PedidosDb;
GO

IF OBJECT_ID('dbo.PedidoItems', 'U') IS NOT NULL DROP TABLE dbo.PedidoItems;
IF OBJECT_ID('dbo.Pedidos', 'U') IS NOT NULL DROP TABLE dbo.Pedidos;
IF OBJECT_ID('dbo.StockAuditoria', 'U') IS NOT NULL DROP TABLE dbo.StockAuditoria;
IF OBJECT_ID('dbo.Productos', 'U') IS NOT NULL DROP TABLE dbo.Productos;
IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL DROP TABLE dbo.Clientes;
GO

CREATE TABLE dbo.Clientes
(
    ClienteId INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(120) NOT NULL,
    Telefono NVARCHAR(30) NULL,
    Activo BIT NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT (1),
    FechaAlta DATETIME2 NOT NULL CONSTRAINT DF_Clientes_FechaAlta DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_Clientes PRIMARY KEY (ClienteId),
    CONSTRAINT UQ_Clientes_Email UNIQUE (Email)
);
GO

CREATE TABLE dbo.Productos
(
    ProductoId INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    StockMinimo INT NOT NULL CONSTRAINT DF_Productos_StockMinimo DEFAULT (5),
    Activo BIT NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT (1),
    FechaAlta DATETIME2 NOT NULL CONSTRAINT DF_Productos_FechaAlta DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_Productos PRIMARY KEY (ProductoId),
    CONSTRAINT CK_Productos_Precio CHECK (Precio > 0),
    CONSTRAINT CK_Productos_Stock CHECK (Stock >= 0),
    CONSTRAINT CK_Productos_StockMinimo CHECK (StockMinimo >= 0)
);
GO

CREATE TABLE dbo.Pedidos
(
    PedidoId INT IDENTITY(1,1) NOT NULL,
    ClienteId INT NOT NULL,
    Fecha DATETIME2 NOT NULL CONSTRAINT DF_Pedidos_Fecha DEFAULT (SYSDATETIME()),
    Estado NVARCHAR(20) NOT NULL CONSTRAINT DF_Pedidos_Estado DEFAULT ('Pendiente'),
    Total DECIMAL(18,2) NOT NULL CONSTRAINT DF_Pedidos_Total DEFAULT (0),
    CONSTRAINT PK_Pedidos PRIMARY KEY (PedidoId),
    CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY (ClienteId) REFERENCES dbo.Clientes(ClienteId),
    CONSTRAINT CK_Pedidos_Estado CHECK (Estado IN ('Pendiente', 'Facturado', 'Cancelado')),
    CONSTRAINT CK_Pedidos_Total CHECK (Total >= 0)
);
GO

CREATE TABLE dbo.PedidoItems
(
    PedidoItemId INT IDENTITY(1,1) NOT NULL,
    PedidoId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    Subtotal AS (Cantidad * PrecioUnitario) PERSISTED,
    CONSTRAINT PK_PedidoItems PRIMARY KEY (PedidoItemId),
    CONSTRAINT FK_PedidoItems_Pedidos FOREIGN KEY (PedidoId) REFERENCES dbo.Pedidos(PedidoId),
    CONSTRAINT FK_PedidoItems_Productos FOREIGN KEY (ProductoId) REFERENCES dbo.Productos(ProductoId),
    CONSTRAINT CK_PedidoItems_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_PedidoItems_PrecioUnitario CHECK (PrecioUnitario > 0)
);
GO

CREATE TABLE dbo.StockAuditoria
(
    StockAuditoriaId INT IDENTITY(1,1) NOT NULL,
    ProductoId INT NOT NULL,
    StockAnterior INT NOT NULL,
    StockNuevo INT NOT NULL,
    Diferencia INT NOT NULL,
    Motivo NVARCHAR(100) NOT NULL,
    Fecha DATETIME2 NOT NULL CONSTRAINT DF_StockAuditoria_Fecha DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_StockAuditoria PRIMARY KEY (StockAuditoriaId),
    CONSTRAINT FK_StockAuditoria_Productos FOREIGN KEY (ProductoId) REFERENCES dbo.Productos(ProductoId)
);
GO

CREATE INDEX IX_Pedidos_ClienteId ON dbo.Pedidos(ClienteId);
CREATE INDEX IX_PedidoItems_PedidoId ON dbo.PedidoItems(PedidoId);
CREATE INDEX IX_PedidoItems_ProductoId ON dbo.PedidoItems(ProductoId);
CREATE INDEX IX_Productos_Activo_Stock ON dbo.Productos(Activo, Stock);
GO
