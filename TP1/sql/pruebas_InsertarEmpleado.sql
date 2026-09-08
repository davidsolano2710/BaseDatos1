USE BDTareaProgramada1;
GO

-- Ejecutar en la base local de desarrollo después de crear el SP.
-- Se revierten las filas de prueba; IDENTITY puede dejar saltos.
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Nombre VARCHAR(128) = 'Prueba-' + TRANSLATE(
        CONVERT(VARCHAR(36), NEWID()),
        '0123456789',
        'abcdefghij'
    );
    DECLARE @Codigo INT;
    DECLARE @CantidadInicial INT;

    SELECT @CantidadInicial = COUNT(E.Id)
    FROM dbo.Empleado AS E;

    EXEC dbo.sp_InsertarEmpleado
        @inNombre = @Nombre
        , @inSalario = '123456.7891'
        , @outCodigo = @Codigo OUTPUT;

    IF @Codigo <> 0
        THROW 51000, 'Falló la inserción válida.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Empleado AS E
        WHERE E.Nombre = @Nombre
            AND E.Salario = CONVERT(MONEY, 123456.7891)
    )
        THROW 51001, 'No se guardó el salario exacto.', 1;

    EXEC dbo.sp_InsertarEmpleado
        @inNombre = @Nombre
        , @inSalario = '200000'
        , @outCodigo = @Codigo OUTPUT;

    IF @Codigo <> 1
        THROW 51002, 'No se rechazó el duplicado.', 1;

    EXEC dbo.sp_InsertarEmpleado
        @inNombre = @Nombre
        , @inSalario = '999999999999999999999999'
        , @outCodigo = @Codigo OUTPUT;

    IF @Codigo <> 2
        THROW 51003, 'No se rechazó el salario fuera de rango.', 1;

    EXEC dbo.sp_InsertarEmpleado
        @inNombre = @Nombre
        , @inSalario = '1.23456'
        , @outCodigo = @Codigo OUTPUT;

    IF @Codigo <> 2
        THROW 51004, 'No se rechazó el exceso de decimales.', 1;

    IF (SELECT COUNT(E.Id) FROM dbo.Empleado AS E) <> @CantidadInicial + 1
        THROW 51005, 'Se insertaron filas adicionales inesperadas.', 1;

    ROLLBACK TRANSACTION;
    PRINT 'Pruebas correctas. Las filas de prueba se revirtieron.';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;

SET NOCOUNT OFF;
GO

