using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using tareaProgramada1.Models;

namespace tareaProgramada1.Services
{
    public class EmpleadoService
    {
        private readonly string _connectionString;

        public EmpleadoService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para listar empleados usando el SP
        public List<Empleado> ListarEmpleados(out int codigoResultado)
        {
            var empleados = new List<Empleado>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(
                "dbo.sp_ListarEmpleados",
                conn);

            cmd.CommandType = CommandType.StoredProcedure;

            var codigo = cmd.Parameters.Add(
                "@outCodigo",
                SqlDbType.Int);

            codigo.Direction = ParameterDirection.Output;

            conn.Open();

            // El parámetro OUTPUT se puede consultar después de cerrar el lector.
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    empleados.Add(new Empleado
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Salario = reader.GetDecimal(2)
                    });
                }
            }

            codigoResultado = Convert.ToInt32(codigo.Value);

            return empleados;
        }

        // La aplicación transmite parámetros; las reglas de datos se resuelven en SQL.
        public int InsertarEmpleado(string nombre, string salario)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("dbo.sp_InsertarEmpleado", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@inNombre", SqlDbType.VarChar, -1).Value = nombre;
            cmd.Parameters.Add("@inSalario", SqlDbType.VarChar, -1).Value = salario;

            var codigo = cmd.Parameters.Add("@outCodigo", SqlDbType.Int);
            codigo.Direction = ParameterDirection.Output;

            conn.Open();
            cmd.ExecuteNonQuery();
            return (int)codigo.Value;
        }
    }
}

