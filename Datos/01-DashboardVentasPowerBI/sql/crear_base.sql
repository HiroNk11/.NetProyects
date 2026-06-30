CREATE DATABASE VentasPowerBIDb;
GO

USE VentasPowerBIDb;
GO

CREATE TABLE Clientes
(
    ClienteId INT PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Segmento NVARCHAR(50) NOT NULL,
    Provincia NVARCHAR(50) NOT NULL
);

CREATE TABLE Productos
(
    ProductoId INT PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Categoria NVARCHAR(50) NOT NULL,
    Costo DECIMAL(18, 2) NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL
);

CREATE TABLE Ventas
(
    VentaId INT PRIMARY KEY,
    Fecha DATE NOT NULL,
    ClienteId INT NOT NULL,
    Canal NVARCHAR(50) NOT NULL,
    Vendedor NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(ClienteId)
);

CREATE TABLE DetalleVentas
(
    VentaId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18, 2) NOT NULL,
    CostoUnitario DECIMAL(18, 2) NOT NULL,
    CONSTRAINT PK_DetalleVentas PRIMARY KEY (VentaId, ProductoId),
    CONSTRAINT FK_DetalleVentas_Ventas FOREIGN KEY (VentaId) REFERENCES Ventas(VentaId),
    CONSTRAINT FK_DetalleVentas_Productos FOREIGN KEY (ProductoId) REFERENCES Productos(ProductoId)
);
