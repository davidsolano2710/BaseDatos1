USE BDTareaProgramada1;
GO

CREATE OR ALTER PROCEDURE dbo.sp_InsertarEmpleado
    @inNombre VARCHAR(MAX)
    , @inSalario VARCHAR(MAX)
    , @outCodigo INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- 0: éxito; 1: nombre duplicado; 2: datos inválidos; 3: error inesperado.
    -- Se recibe el salario como texto para validar el rango MONEY en la BD.
    SET @outCodigo = 3;

    BEGIN TRY
        DECLARE @Nombre VARCHAR(MAX) = LTRIM(RTRIM(@inNombre));
        DECLARE @Salario MONEY = TRY_CONVERT(MONEY, @inSalario);

        IF @Nombre IS NULL
            OR LEN(@Nombre) = 0
            OR DATALENGTH(@Nombre) > 128
            OR @Nombre COLLATE Latin1_General_100_BIN2 LIKE '%[^a-zA-ZáéíóúÁÉÍÓÚñÑüÜ -]%'
            OR @Nombre COLLATE Latin1_General_100_BIN2 NOT LIKE '%[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ]%'
            OR @inSalario IS NULL
            OR LEN(@inSalario) = 0
            OR @Salario IS NULL
            OR @inSalario COLLATE Latin1_General_100_BIN2 LIKE '%[^0-9.]%'
            OR @inSalario NOT LIKE '%[0-9]%'
            OR LEN(@inSalario) - LEN(REPLACE(@inSalario, '.', '')) > 1
            OR LEFT(@inSalario, 1) = '.'
            OR RIGHT(@inSalario, 1) = '.'
            OR (CHARINDEX('.', @inSalario) > 0
                AND LEN(@inSalario) - CHARINDEX('.', @inSalario) > 4)
        BEGIN
            SET @outCodigo = 2;
        END
        ELSE
        BEGIN
            BEGIN TRANSACTION;

            -- Bloqueo para impedir que dos inserciones simultáneas admitan
            -- el mismo nombre. No se utiliza un índice UNIQUE.
            IF EXISTS
            (
                SELECT 1
                FROM dbo.Empleado AS E WITH (TABLOCKX, HOLDLOCK)
                WHERE E.Nombre = @Nombre
            )
            BEGIN
                SET @outCodigo = 1;
            END
            ELSE
            BEGIN
                INSERT INTO dbo.Empleado
                (
                    Nombre
                    , Salario
                )
                VALUES
                (
                    @Nombre
                    , @Salario
                );

                SET @outCodigo = 0;
            END;

            COMMIT TRANSACTION;
        END;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        SET @outCodigo = 3;
    END CATCH;

    SET NOCOUNT OFF;
END;
GO

