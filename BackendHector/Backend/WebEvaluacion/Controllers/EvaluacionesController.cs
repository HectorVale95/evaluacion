using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ClbModEvaluacion;
using ClbNegEvaluacion;

namespace WebEvaluacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionesController : ControllerBase
    {
        private readonly ClsNegEvaluaciones _negEvaluaciones;
        private readonly string _connectionString;

        public EvaluacionesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _negEvaluaciones = new ClsNegEvaluaciones(_connectionString);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClsModEvaluacion>> GetAll()
        {
            try
            {
                return Ok(_negEvaluaciones.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las evaluaciones: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ClsModEvaluacion> GetById(int id)
        {
            try
            {
                var evaluacion = _negEvaluaciones.GetById(id);
                if (evaluacion == null)
                    return NotFound($"No se encontró la evaluación con ID {id}");

                return Ok(evaluacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la evaluación: {ex.Message}");
            }
        }

        [HttpGet("empleado/{idEmpleado}")]
        public ActionResult<IEnumerable<ClsModEvaluacion>> GetByEmpleado(int idEmpleado)
        {
            try
            {
                return Ok(_negEvaluaciones.GetByEmpleado(idEmpleado));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las evaluaciones del empleado: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<int> Create([FromBody] ClsModEvaluacion evaluacion)
        {
            try
            {
                Console.WriteLine("========== CREAR EVALUACION ==========");
                Console.WriteLine($"IdEmpleado: {evaluacion.IdEmpleado}");
                Console.WriteLine($"FechaEvaluacion: {evaluacion.FechaEvaluacion}");
                Console.WriteLine($"Productividad: {evaluacion.Productividad}");
                Console.WriteLine($"Puntualidad: {evaluacion.Puntualidad}");
                Console.WriteLine($"CalidadTrabajo: {evaluacion.CalidadTrabajo}");
                Console.WriteLine($"Comunicacion: {evaluacion.Comunicacion}");
                Console.WriteLine($"DisposicionAprender: {evaluacion.DisposicionAprender}");
                Console.WriteLine($"Honestidad: {evaluacion.Honestidad}");
                Console.WriteLine($"Iniciativa: {evaluacion.Iniciativa}");
                Console.WriteLine($"IntegracionEquipo: {evaluacion.IntegracionEquipo}");
                Console.WriteLine($"Comentarios: {evaluacion.Comentarios}");
                var id = _negEvaluaciones.Create(evaluacion);
                return CreatedAtAction(nameof(GetById), new { id = id }, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR CREAR EVALUACION] {ex.Message}");
                return StatusCode(500, $"Error al crear la evaluación: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClsModEvaluacion evaluacion)
        {
            try
            {
                if (id != evaluacion.IdEvaluacion)
                    return BadRequest("El ID de la evaluación no coincide");

                _negEvaluaciones.Update(evaluacion);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la evaluación: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _negEvaluaciones.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la evaluación: {ex.Message}");
            }
        }
    }
} 