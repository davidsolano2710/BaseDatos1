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
        public List<Empleado> ListarEmpleados()
        {
            var empleados = new List<Empleado>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("sp_ListarEmpleados", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                }
            }

            return empleados;
        }
    }
}
