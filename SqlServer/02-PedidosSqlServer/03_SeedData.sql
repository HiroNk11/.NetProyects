USE PedidosDb;
GO

INSERT INTO dbo.Clientes (Nombre, Email, Telefono)
VALUES
('Consumidor Final', 'consumidor@example.com', NULL),
('Nicolas Gomez', 'nico@example.com', '1122334455'),
('Maria Lopez', 'maria@example.com', '1166778899');
GO

INSERT INTO dbo.Productos (Nombre, Precio, Stock, StockMinimo)
VALUES
('Mouse Logitech', 15000.00, 20, 5),
('Teclado mecanico', 45000.00, 12, 3),
('Monitor 24 pulgadas', 180000.00, 6, 2),
('Notebook oficina', 950000.00, 3, 1);
GO
