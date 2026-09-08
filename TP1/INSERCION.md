# Inserción de empleados (Registro de implementación por GPT)

Implementación preparada con asistencia de ChatGPT/Codex. Compilada y probada desde un repositorio local en el Windows en el equipo de David Salazar.

## Responsabilidades

- InsertarEmpleadoViewModel: presentación, campos obligatorios y formatos.
  Nombres españoles con letras, espacios y guiones; salarios con punto y hasta
  cuatro decimales. Se permiten espacios por los ejemplos de nombres completos.
- Views/Empleado/Insertar.cshtml: formulario, mensajes, Insertar y Regresar.
- EmpleadoController: validación de presentación, envío y traducción del código.
- EmpleadoService: invocación del SP con parámetros tipados y lectura de OUTPUT.
- dbo.sp_InsertarEmpleado: validación de datos y rango MONEY, duplicados e inserción.
  El salario viaja como texto para que el rango y la conversión a MONEY se resuelvan
  en SQL Server, sin depender de la configuración decimal regional de C#.

Códigos OUTPUT: 0 éxito; 1 duplicado; 2 datos inválidos; 3 error inesperado.
Se comprueba el duplicado programáticamente, sin UNIQUE ni cursores.
La transacción bloquea la tabla durante la comprobación e inserción para evitar
duplicados simultáneos. Es una solución simple para la pequeña tabla de esta tarea.
La comparación de nombres utiliza la intercalación de la columna existente.
El formulario utiliza protección antifalsificación y parámetros SQL, sin concatenar consultas.
Tras el éxito se redirige al listado actualizado, donde se muestra el mensaje de éxito.
El error de duplicado conserva el formulario y los valores para corregirlos.

## Pruebas realizadas en el PC

Ejecutar TP1/sql/pruebas_InsertarEmpleado.sql en desarrollo.
Comprueba salario con cuatro decimales, duplicado, exceso de decimales,
desbordamiento MONEY y cantidad de filas. Revierte los datos de prueba,
aunque el contador IDENTITY puede avanzar; los saltos son normales.

Pruebas manuales de la web:

| Caso | Resultado |
| --- | --- |
| Regresar sin insertar | Vuelve al listado sin agregar filas |
| Campos vacíos | Mensajes de campos obligatorios |
| Nombre con números | Error de formato |
| Salario con coma, letras o cinco decimales | Error de formato |
| Nombre nuevo y salario 250000.1234 | Una fila nueva, listado actualizado y mensaje de éxito |
| Mismo nombre otra vez | Nombre de Empleado ya existe; permanece en el formulario |
| Actualizar el listado tras éxito | No repite la inserción |
| Nombre de más de 128 caracteres | Rechazo de la BD |
| Salario numérico fuera del rango MONEY | Rechazo de la BD |
