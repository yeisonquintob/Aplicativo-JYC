USE [MercanciasJyC]
GO
/****** Object:  StoredProcedure [dbo].[sp_AgregarDetallePedido]    Script Date: 22/09/2024 7:57:42 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Procedimiento para agregar un detalle de pedido
ALTER PROCEDURE [dbo].[sp_AgregarDetallePedido]
    @PedidoID INT,
    @ProductoID INT,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @PrecioUnitario DECIMAL(10, 2);
        DECLARE @StockDisponible INT;

        SELECT @PrecioUnitario = Precio, @StockDisponible = Stock
        FROM Productos
        WHERE ProductoID = @ProductoID;

        IF @Cantidad > @StockDisponible
            THROW 50008, 'No hay suficiente stock disponible.', 1;

        INSERT INTO DetallesPedido (PedidoID, ProductoID, Cantidad, PrecioUnitario)
        VALUES (@PedidoID, @ProductoID, @Cantidad, @PrecioUnitario);

        UPDATE Productos
        SET Stock = Stock - @Cantidad
        WHERE ProductoID = @ProductoID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        THROW;
    END CATCH
END
