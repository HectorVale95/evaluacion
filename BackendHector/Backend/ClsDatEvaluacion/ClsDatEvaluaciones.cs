using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ClbModEvaluacion;
using Dapper;

namespace ClbDatEvaluacion
{
    public class ClsDatEvaluaciones
    {
        private readonly string _connectionString;

        public ClsDatEvaluaciones(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ClsModEvaluacion> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<ClsModEvaluacion>("SpdObtenerEvaluaciones", commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public ClsModEvaluacion GetById(int idEvaluacion)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdEvaluacion", idEvaluacion);
                return db.QueryFirstOrDefault<ClsModEvaluacion>("SpdObtenerEvaluacion", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public List<ClsModEvaluacion> GetByEmpleado(int idEmpleado)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdEmpleado", idEmpleado);
                return db.Query<ClsModEvaluacion>("SpdObtenerEvaluaciones", parameters, commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public int Create(ClsModEvaluacion evaluacion)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdEmpleado", evaluacion.IdEmpleado);
                parameters.Add("@FechaEvaluacion", evaluacion.FechaEvaluacion, System.Data.DbType.Date);
                parameters.Add("@Productividad", evaluacion.Productividad);
                parameters.Add("@Puntualidad", evaluacion.Puntualidad);
                parameters.Add("@CalidadTrabajo", evaluacion.CalidadTrabajo);
                parameters.Add("@Comunicacion", evaluacion.Comunicacion);
                parameters.Add("@DisposicionAprender", evaluacion.DisposicionAprender);
                parameters.Add("@Honestidad", evaluacion.Honestidad);
                parameters.Add("@Iniciativa", evaluacion.Iniciativa);
                parameters.Add("@IntegracionEquipo", evaluacion.IntegracionEquipo);
                parameters.Add("@Comentarios", evaluacion.Comentarios);

                return db.QueryFirstOrDefault<int>("SpdCrearEvaluacion", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(ClsModEvaluacion evaluacion)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdEvaluacion", evaluacion.IdEvaluacion);
                parameters.Add("@IdEmpleado", evaluacion.IdEmpleado);
                parameters.Add("@FechaEvaluacion", evaluacion.FechaEvaluacion, System.Data.DbType.Date);
                parameters.Add("@Productividad", evaluacion.Productividad);
                parameters.Add("@Puntualidad", evaluacion.Puntualidad);
                parameters.Add("@CalidadTrabajo", evaluacion.CalidadTrabajo);
                parameters.Add("@Comunicacion", evaluacion.Comunicacion);
                parameters.Add("@DisposicionAprender", evaluacion.DisposicionAprender);
                parameters.Add("@Honestidad", evaluacion.Honestidad);
                parameters.Add("@Iniciativa", evaluacion.Iniciativa);
                parameters.Add("@IntegracionEquipo", evaluacion.IntegracionEquipo);
                parameters.Add("@Comentarios", evaluacion.Comentarios);

                db.Execute("SpdActualizarEvaluacion", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int idEvaluacion)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdEvaluacion", idEvaluacion);
                db.Execute("SpdEliminarEvaluacion", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
} 