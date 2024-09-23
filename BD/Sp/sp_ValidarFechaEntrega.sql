USE [MercanciasJyC]
GO
/****** Object:  StoredProcedure [dbo].[sp_ValidarFechaEntrega]    Script Date: 22/09/2024 7:58:35 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Procedimiento para validar la fecha de entrega
ALTER PROCEDURE [dbo].[sp_ValidarFechaEntrega]
    @FechaPedido DATETIME,
    @FechaEntregaProgramada DATE
AS
BEGIN
    DECLARE @DiaSemana INT = DATEPART(WEEKDAY, @FechaEntregaProgramada);
    DECLARE @HoraPedido TIME = CAST(@FechaPedido AS TIME);
    DECLARE @FechaLimite DATETIME = DATEADD(DAY, -3, @FechaEntregaProgramada);

    IF @DiaSemana = 1 AND (@FechaPedido < DATEADD(DAY, -2, @FechaEntregaProgramada) OR @FechaPedido > DATEADD(DAY, -1, @FechaEntregaProgramada))
        THROW 50001, 'Los pedidos para entrega el lunes deben realizarse entre el viernes al mediodía y el sábado al mediodía.', 1;
    ELSE IF @DiaSemana = 2 AND (@FechaPedido < DATEADD(DAY, -2, @FechaEntregaProgramada) OR @FechaPedido > DATEADD(DAY, -1, @FechaEntregaProgramada))
        THROW 50002, 'Los pedidos para entrega el martes deben realizarse entre el sábado después del mediodía y el domingo al mediodía.', 1;
    ELSE IF @DiaSemana = 3 AND (@FechaPedido < DATEADD(DAY, -2, @FechaEntregaProgramada) OR @FechaPedido > DATEADD(DAY, -1, @FechaEntregaProgramada))
        THROW 50003, 'Los pedidos para entrega el miércoles deben realizarse entre el domingo después del mediodía y el lunes al mediodía.', 1;
    ELSE IF @DiaSemana = 4 AND (@FechaPedido < DATEADD(DAY, -2, @FechaEntregaProgramada) OR @FechaPedido > DATEADD(DAY, -1, @FechaEntregaProgramada))
        THROW 50004, 'Los pedidos para entrega el jueves deben realizarse entre el lunes después del mediodía y el martes al mediodía.', 1;
    ELSE IF @DiaSemana = 5 AND (@FechaPedido < DATEADD(DAY, -2, @FechaEntregaProgramada) OR @FechaPedido > DATEADD(DAY, -1, @FechaEntregaProgramada))
        THROW 50005, 'Los pedidos para entrega el viernes deben realizarse entre el martes después del mediodía y el miércoles al mediodía.', 1;
    ELSE IF @DiaSemana = 6 AND (@FechaPedido < DATEADD(DAY, -2, @FechaEntregaProgramada) OR @FechaPedido > DATEADD(DAY, -1, @FechaEntregaProgramada))
        THROW 50006, 'Los pedidos para entrega el sábado deben realizarse entre el miércoles después del mediodía y el jueves al mediodía.', 1;
    ELSE IF @DiaSemana = 7
        THROW 50007, 'No se realizan entregas los domingos.', 1;
END
