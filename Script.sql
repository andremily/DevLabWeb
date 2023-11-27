----------------- TABLAS------------------------------------

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
	[Id] INT IDENTITY(1, 1) PRIMARY KEY, 
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
	[Id] INT IDENTITY(1, 1) PRIMARY KEY, 
    [IdFactura] INT NOT NULL, 
    [IdProducto] INT NOT NULL, 
    [CantidadDeProducto] INT NOT NULL, 
    [PrecioUnitarioProducto] DECIMAL(18, 2) NOT NULL, 
    [SubtotalProducto] DECIMAL(18, 2) NOT NULL, 
    [Notas] VARCHAR(200) NOT NULL,
	FOREIGN KEY (IdFactura) REFERENCES TblFacturas(Id),
	FOREIGN KEY (IdProducto) REFERENCES CatProductos(Id)
)
--------------------INSERT DE LAS TABLAS --------------------------------------------------------------------

  INSERT INTO CatTipoCliente(ID, TipoCliente)VALUES(1, "Empresa");
  INSERT INTO CatTipoCliente(ID, TipoCliente)VALUES(2, "Persona");

  INSERT INTO TblClientes(Id, RazonSocial, IdTipoCliente, FechaCreacion, RFC)VALUES(1, "EMPRESA PRUEBA", 1,GETDATE(),"PRUEBA" );
  INSERT INTO TblClientes(Id, RazonSocial, IdTipoCliente, FechaCreacion, RFC)VALUES(2, "EMPRESA PRUEBA2", 1,GETDATE(),"PRUEBA 2" );
  INSERT INTO TblClientes(Id, RazonSocial, IdTipoCliente, FechaCreacion, RFC)VALUES(3, "JOHANA AGUIRRE", 2,GETDATE(),"JOHANA" );


----------------------PROCEDIMIENTOS ALMACENADOS-------------------------------------------------------------

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


CREATE OR ALTER  PROCEDURE GuardarFactura 
@FechaEmisionFactura DATETIME,
@IdCliente INT,
@NumeroFactura INT ,
@NumeroTotalArticulos INT,
@SubTotalFactura  DECIMAL(18, 2),
@TotalImpuesto DECIMAL(18, 2) ,
@TotalFactura DECIMAL(18, 2)
AS
BEGIN
 SET NOCOUNT ON; 
    INSERT INTO TblFacturas (FechaEmisionFactura, IdCliente, NumeroFactura, NumeroTotalArticulos, SubTotalFactura, TotalImpuesto, TotalFactura)
    VALUES 
    (@FechaEmisionFactura, @IdCliente, @NumeroFactura, @NumeroTotalArticulos, @SubTotalFactura, @TotalImpuesto, @TotalFactura);
    SELECT SCOPE_IDENTITY() AS IdFactura;
END;

GO 
CREATE OR ALTER PROCEDURE GuardarDetalleFactura 
@IdFactura INT,
@IdProducto INT,
@CantidadDeProducto INT,
@PrecioUnitarioProducto  DECIMAL(18, 2),
@SubtotalProducto DECIMAL(18, 2),
@Notas VARCHAR(200)
AS
BEGIN
    INSERT INTO TblDetallesFactura(IdFactura, IdProducto, CantidadDeProducto, PrecioUnitarioProducto, SubtotalProducto, Notas)
    VALUES 
    (@IdFactura, @IdProducto, @CantidadDeProducto, @PrecioUnitarioProducto, @SubtotalProducto, @Notas);
END;


CREATE OR ALTER PROCEDURE ConsultarProductos
AS
BEGIN
    SELECT
        Id, 
        NombreProducto, 
        PrecioUnitario,
        ext,
        CONVERT(VARCHAR(MAX), CAST(ImagenProducto AS VARBINARY(MAX)), 1)  AS ImagenProducto
    FROM
        CatProductos;
END;

CREATE OR ALTER PROCEDURE ConsultaNumeroFactura 
@NumeroFactura int = null,
@IdCliente int = null
AS
BEGIN
    SELECT NumeroFactura, FechaEmisionFactura, TotalFactura, IdCliente
    FROM TblFacturas
    Where 
	 (@NumeroFactura IS NULL OR NumeroFactura = @NumeroFactura)
        AND
     (@IdCliente IS NULL OR IdCliente = @IdCliente)
      
    
END;

CREATE OR ALTER PROCEDURE ConsultaClientes
AS
BEGIN
    SELECT Id, RazonSocial
    FROM TblClientes;
END;


