using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ClbModEvaluacion;
using Dapper;

namespace ClbDatEvaluacion
{
    public class ClsDatEmpleados
    {
        private readonly string _connectionString;

        public ClsDatEmpleados(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ClsModEmpleado> GetAll()
        {
      try
      {
        using (IDbConnection db = new SqlConnection(_connectionString))
        {
          var result = db.Query<ClsModEmpleado>("SpdObtenerEmpleados", commandType: CommandType.StoredProcedure).AsList();
          Console.WriteLine($"Empleados obtenidos: {result.Count}");
          return result;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("ERROR en GetAll Empleados: " + ex.ToString());
        throw;
      }
    }

        public ClsModEmpleado GetById(int idEmpleado)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdEmpleado", idEmpleado);
                return db.QueryFirstOrDefault<ClsModEmpleado>("SpdObtenerEmpleado", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public int Create(ClsModEmpleado empleado)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CodigoEmpleado", empleado.CodigoEmpleado);
                parameters.Add("@Nombre", empleado.Nombre);
                parameters.Add("@ApellidoPaterno", empleado.ApellidoPaterno);
                parameters.Add("@ApellidoMaterno", empleado.ApellidoMaterno);
                parameters.Add("@FechaNacimiento", empleado.FechaNacimiento);
                parameters.Add("@FechaInicioContrato", empleado.FechaInicioContrato);
                parameters.Add("@IdPuesto", empleado.IdPuesto);

                return db.QueryFirstOrDefault<int>("SpdCrearEmpleado", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(ClsModEmpleado empleado)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdEmpleado", empleado.IdEmpleado);
                parameters.Add("@CodigoEmpleado", empleado.CodigoEmpleado);
                parameters.Add("@Nombre", empleado.Nombre);
                parameters.Add("@ApellidoPaterno", empleado.ApellidoPaterno);
                parameters.Add("@ApellidoMaterno", empleado.ApellidoMaterno);
                parameters.Add("@FechaNacimiento", empleado.FechaNacimiento);
                parameters.Add("@FechaInicioContrato", empleado.FechaInicioContrato);
                parameters.Add("@IdPuesto", empleado.IdPuesto);

                db.Execute("SpdActualizarEmpleado", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int idEmpleado)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdEmpleado", idEmpleado);
                db.Execute("SpdEliminarEmpleado", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
} 
