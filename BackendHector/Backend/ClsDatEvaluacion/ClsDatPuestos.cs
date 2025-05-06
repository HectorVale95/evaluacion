using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ClbModEvaluacion;
using Dapper;

namespace ClbDatEvaluacion
{
    public class ClsDatPuestos
    {
        private readonly string _connectionString;

        public ClsDatPuestos(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ClsModPuesto> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<ClsModPuesto>("SpdObtenerPuestos", commandType: CommandType.StoredProcedure).AsList();
            }
        }

        public ClsModPuesto GetById(int idPuesto)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdPuesto", idPuesto);
                return db.QueryFirstOrDefault<ClsModPuesto>("SpdObtenerPuesto", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public int Create(ClsModPuesto puesto)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@NombrePuesto", puesto.NombrePuesto);

                return db.QueryFirstOrDefault<int>("SpdCrearPuesto", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Update(ClsModPuesto puesto)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdPuesto", puesto.IdPuesto);
                parameters.Add("@NombrePuesto", puesto.NombrePuesto);

                db.Execute("SpdActualizarPuesto", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int idPuesto)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdPuesto", idPuesto);
                db.Execute("SpdEliminarPuesto", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
} 