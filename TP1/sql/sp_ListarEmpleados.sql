CREATE PROCEDURE sp_ListarEmpleados
AS
BEGIN
    SELECT Id, Nombre, Salario
    FROM Empleado
    ORDER BY Nombre ASC;
END;
