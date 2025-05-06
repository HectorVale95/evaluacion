using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClbModEvaluacion;
using ClbDatEvaluacion;
using ExcelDataReader;
using System.IO;
using System.Data;

namespace ClbNegEvaluacion
{
  public class ClsNegImportacion
  {
    private readonly ClsDatImportacion _datImportacion;

    public ClsNegImportacion(ClsDatImportacion datImportacion)
    {
      _datImportacion = datImportacion;
    }

    public async Task<int> ImportarEvaluacionesDesdeExcel(Stream fileStream)
    {
      try
      {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var reader = ExcelReaderFactory.CreateReader(fileStream))
        {
          var evaluaciones = new List<ClsModEvaluacion>();
          var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
          {
            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
            {
              UseHeaderRow = true
            }
          });

          var table = dataSet.Tables[0];
          foreach (DataRow row in table.Rows)
          {
            // Leemos primero la cadena cruda:
            var rawFecha = row["FechaEvaluacion"]?.ToString();

            // Intentamos parsear a DateTime
            DateTime? fechaEvaluacion = null;
            if (row["FechaEvaluacion"] is DateTime dt)
            {
              fechaEvaluacion = dt;
            }
            else if (!string.IsNullOrWhiteSpace(rawFecha)
                     && DateTime.TryParse(rawFecha, out var parsedDt))
            {
              fechaEvaluacion = parsedDt;
            }

            var evaluacion = new ClsModEvaluacion
            {
              IdEmpleado = Convert.ToInt32(row["IdEmpleado"]),
              FechaEvaluacion = fechaEvaluacion,   // ahora DateTime?
              Productividad = Convert.ToInt32(row["Productividad"]),
              Puntualidad = Convert.ToInt32(row["Puntualidad"]),
              CalidadTrabajo = Convert.ToInt32(row["CalidadTrabajo"]),
              Comunicacion = Convert.ToInt32(row["Comunicacion"]),
              DisposicionAprender = Convert.ToInt32(row["DisposicionAprender"]),
              Honestidad = Convert.ToInt32(row["Honestidad"]),
              Iniciativa = Convert.ToInt32(row["Iniciativa"]),
              IntegracionEquipo = Convert.ToInt32(row["IntegracionEquipo"]),
              Comentarios = row["Comentarios"]?.ToString()
            };

            ValidateEvaluacion(evaluacion);
            evaluaciones.Add(evaluacion);
          }

          return await _datImportacion.ImportarEvaluaciones(evaluaciones);
        }
      }
      catch (Exception ex)
      {
        throw new Exception($"Error al importar el archivo Excel: {ex.Message}", ex);
      }
    }

    private void ValidateEvaluacion(ClsModEvaluacion evaluacion)
    {
      if (evaluacion.IdEmpleado <= 0)
        throw new ArgumentException("El ID del empleado debe ser mayor que 0");

      // Si hay fecha, no puede ser futura
      if (evaluacion.FechaEvaluacion.HasValue
          && evaluacion.FechaEvaluacion.Value.Date > DateTime.Now.Date)
      {
        throw new ArgumentException("La fecha de evaluación no puede ser futura");
      }

      ValidateScore(evaluacion.Productividad, "Productividad");
      ValidateScore(evaluacion.Puntualidad, "Puntualidad");
      ValidateScore(evaluacion.CalidadTrabajo, "Calidad del trabajo");
      ValidateScore(evaluacion.Comunicacion, "Comunicación");
      ValidateScore(evaluacion.DisposicionAprender, "Disposición para aprender");
      ValidateScore(evaluacion.Honestidad, "Honestidad");
      ValidateScore(evaluacion.Iniciativa, "Iniciativa");
      ValidateScore(evaluacion.IntegracionEquipo, "Integración con el equipo");
    }

    private void ValidateScore(int score, string fieldName)
    {
      if (score < 0 || score > 100)
        throw new ArgumentException($"El puntaje de {fieldName} debe estar entre 0 y 100");
    }
  }
}
