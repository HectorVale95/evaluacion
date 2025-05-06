using System;
using System.Collections.Generic;
using ClbModEvaluacion;
using ClbDatEvaluacion;

namespace ClbNegEvaluacion
{
  public class ClsNegEvaluaciones
  {
    private readonly ClsDatEvaluaciones _datEvaluaciones;

    public ClsNegEvaluaciones(string connectionString)
    {
      _datEvaluaciones = new ClsDatEvaluaciones(connectionString);
    }

    public List<ClsModEvaluacion> GetAll()
    {
      try
      {
        return _datEvaluaciones.GetAll();
      }
      catch (Exception ex)
      {
        throw new Exception($"Error al obtener las evaluaciones: {ex.Message}", ex);
      }
    }

    public ClsModEvaluacion GetById(int idEvaluacion)
    {
      try
      {
        return _datEvaluaciones.GetById(idEvaluacion);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error al obtener la evaluación: {ex.Message}", ex);
      }
    }

    public List<ClsModEvaluacion> GetByEmpleado(int idEmpleado)
    {
      try
      {
        return _datEvaluaciones.GetByEmpleado(idEmpleado);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error al obtener las evaluaciones del empleado: {ex.Message}", ex);
      }
    }

    public int Create(ClsModEvaluacion evaluacion)
    {
      try
      {
        ValidateEvaluation(evaluacion);
        return _datEvaluaciones.Create(evaluacion);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error al crear la evaluación: {ex.Message}", ex);
      }
    }

    public void Update(ClsModEvaluacion evaluacion)
    {
      try
      {
        ValidateEvaluation(evaluacion);
        _datEvaluaciones.Update(evaluacion);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error al actualizar la evaluación: {ex.Message}", ex);
      }
    }

    public void Delete(int idEvaluacion)
    {
      try
      {
        _datEvaluaciones.Delete(idEvaluacion);
      }
      catch (Exception ex)
      {
        throw new Exception($"Error al eliminar la evaluación: {ex.Message}", ex);
      }
    }

    private void ValidateEvaluation(ClsModEvaluacion evaluacion)
    {
      if (evaluacion == null)
        throw new ArgumentNullException(nameof(evaluacion), "La evaluación no puede ser nula");

      if (evaluacion.IdEmpleado <= 0)
        throw new ArgumentException("El ID del empleado es inválido");

      if (evaluacion.FechaEvaluacion.HasValue
          && evaluacion.FechaEvaluacion.Value.Date > DateTime.Now.Date)
        throw new ArgumentException("La fecha de evaluación no puede ser futura");

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
