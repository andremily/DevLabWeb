CREATE TABLE [dbo].[CatTipoCliente]
(
    [ID] INT NOT NULL PRIMARY KEY,  
    [TipoCliente] VARCHAR(50) NOT NULL
)
CREATE TABLE [dbo].[TblClientes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [RazonSocial] VARCHAR(200) NOT NULL, 
    [IdTipoCliente] INT NOT NULL, 
    [FechaCreacion] DATE NOT NULL, 
    [RFC] VARCHAR(50) NOT NULL,
	FOREIGN KEY (IdTipoCliente) REFERENCES CatTipoCliente(ID)
)
CREATE TABLE [dbo].[TblFacturas]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [FechaEmisionFactura] DATETIME NOT NULL, 
    [IdCliente] INT NOT NULL, 
    [NumeroFactura] INT NOT NULL, 
    [NumeroTotalArticulos] INT NOT NULL, 
    [SubTotalFactura] DECIMAL(18, 2) NOT NULL, 
    [TotalImpuesto] DECIMAL(18, 2) NOT NULL, 
    [TotalFactura] DECIMAL(18, 2) NOT NULL,
	FOREIGN KEY (IdCliente) REFERENCES TblClientes(Id)
)
CREATE TABLE [dbo].[TblDetallesFactura]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [IdFactura] INT NOT NULL, 
    [IdProducto] INT NOT NULL, 
    [CantidadDeProducto] INT NOT NULL, 
    [PrecioUnitarioProducto] DECIMAL(18, 2) NOT NULL, 
    [SubtotalProducto] DECIMAL(18, 2) NOT NULL, 
    [Notas] VARCHAR(200) NOT NULL,
	FOREIGN KEY (IdFactura) REFERENCES TblFacturas(Id),
	FOREIGN KEY (IdProducto) REFERENCES CatProductos(Id)
)

CREATE PROCEDURE ConsultaClientes
AS
BEGIN
    SELECT Id, RazonSocial
    FROM TblClientes;
END;

CREATE PROCEDURE ConsultarProductos
AS
BEGIN
    SELECT Id, NombreProducto, ImagenProducto, PrecioUnitario, ext
    FROM CatProductos;
END;