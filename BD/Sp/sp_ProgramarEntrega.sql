USE [MercanciasJyC]
GO
/****** Object:  StoredProcedure [dbo].[sp_ProgramarEntrega]    Script Date: 22/09/2024 7:58:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Procedimiento para programar una entrega
ALTER PROCEDURE [dbo].[sp_ProgramarEntrega]
    @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Entregas (PedidoID, EstadoEntrega)
        VALUES (@PedidoID, 'Programada');

        UPDATE Pedidos
        SET EstadoPedido = 'En Proceso'
        WHERE PedidoID = @PedidoID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        THROW;
    END CATCH
END
