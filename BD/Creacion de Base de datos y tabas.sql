-- Crear la base de datos
CREATE DATABASE MercanciasJyC;
GO

USE MercanciasJyC;
GO

-- Tabla de Clientes
CREATE TABLE Clientes (
    ClienteID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(200) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);

-- Tabla de Productos
CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    Precio DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL
);

-- Tabla de Pedidos
CREATE TABLE Pedidos (
    PedidoID INT PRIMARY KEY IDENTITY(1,1),
    ClienteID INT NOT NULL,
    FechaPedido DATETIME NOT NULL,
    FechaEntregaProgramada DATE NOT NULL,
    EstadoPedido NVARCHAR(20) NOT NULL,
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);

-- Tabla de Detalles de Pedido
CREATE TABLE DetallesPedido (
    DetalleID INT PRIMARY KEY IDENTITY(1,1),
    PedidoID INT NOT NULL,
    ProductoID INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (PedidoID) REFERENCES Pedidos(PedidoID),
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);

-- Tabla de Entregas
CREATE TABLE Entregas (
    EntregaID INT PRIMARY KEY IDENTITY(1,1),
    PedidoID INT NOT NULL,
    FechaEntregaReal DATETIME,
    EstadoEntrega NVARCHAR(20) NOT NULL,
    FOREIGN KEY (PedidoID) REFERENCES Pedidos(PedidoID)
);
