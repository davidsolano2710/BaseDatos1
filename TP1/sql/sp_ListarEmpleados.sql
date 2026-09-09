USE BDTareaProgramada1;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ListarEmpleados
    @outCodigo INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Códigos de salida:
    -- 0: listado obtenido correctamente.
    -- 1: ocurrió un error durante la consulta.
    SET @outCodigo = 1;

    BEGIN TRY
        SELECT
            E.Id
            , E.Nombre
            , E.Salario
        FROM dbo.Empleado AS E
        ORDER BY
            E.Nombre ASC;

        SET @outCodigo = 0;
    END TRY
    BEGIN CATCH
        SET @outCodigo = 1;
    END CATCH;

    SET NOCOUNT OFF;
END;
GO