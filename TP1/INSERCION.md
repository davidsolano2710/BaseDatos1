# Inserción de empleados

Implementación preparada con asistencia de ChatGPT/Codex. Pendiente de compilar y
probar en el Windows del equipo: el entorno del asistente no dispone de .NET ni
SQL Server. No se debe registrar como funcionalidad comprobada hasta ejecutar
las pruebas.

## Instalación sobre la base existente

1. Detener la web en Visual Studio (Shift + F5).
2. Extraer el ZIP en una carpeta temporal. Comparar sus archivos con el repositorio local y copiar la carpeta TP1 sobre la raíz BaseDatos1, preservando cualquier cambio local no relacionado. Abrir la solución existente.
3. En SSMS, conectar a la instancia local que contiene BDTareaProgramada1.
4. Abrir TP1/sql/sp_InsertarEmpleado.sql y ejecutar. No volver a cargar los 40 empleados.
5. Revisar appsettings.json: cada integrante usa la instancia que tenga en su PC.
6. Ejecutar la web con F5 y pulsar Insertar Empleado.

El script CREATE OR ALTER puede repetirse para actualizar el procedimiento sin borrar datos.

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

## Pruebas pendientes en el PC

Ejecutar TP1/sql/pruebas_InsertarEmpleado.sql en desarrollo.
Comprueba salario con cuatro decimales, duplicado, exceso de decimales,
desbordamiento MONEY y cantidad de filas. Revierte los datos de prueba,
aunque el contador IDENTITY puede avanzar; los saltos son normales.

Pruebas manuales de la web:

| Caso | Resultado esperado |
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

## Registro del trabajo

La bitácora debe indicar la ayuda recibida, el commit usado, los errores observados,
las pruebas realmente ejecutadas y su resultado. Completar inicio, fin y duración
con los tiempos reales del estudiante. Este archivo no sustituye la entrada en el blog.

El procedimiento de listado anterior todavía requiere revisión de estilo según
la rúbrica (TRY/CATCH, códigos de error, NOCOUNT, alias y dbo). La documentación
formal y el acceso compartido al servidor también quedan pendientes.

La publicación desde el asistente fue rechazada por GitHub con HTTP 403 (permisos de integración). No se creó una rama ni un commit. El ZIP contiene únicamente los archivos nuevos o modificados respecto de main, commit e1f0496cd20964ba931671a0d781f75122a3cbef. No contiene un proyecto independiente ni cambia appsettings.json.

