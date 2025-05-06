using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ClbModEvaluacion;
using System.Collections.Generic;

namespace ClbDatEvaluacion
{
    public class ClsDatImportacion
    {
        private readonly string _connectionString;

        public ClsDatImportacion(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> ImportarEvaluaciones(List<ClsModEvaluacion> evaluaciones)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    db.Open();
                    var importados = 0;

                    foreach (var evaluacion in evaluaciones)
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

                        await db.ExecuteAsync("SpdEvaluaciones_Create", parameters, commandType: CommandType.StoredProcedure);
                        importados++;
                    }

                    return importados;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al importar evaluaciones: {ex.Message}", ex);
            }
        }
    }
} 