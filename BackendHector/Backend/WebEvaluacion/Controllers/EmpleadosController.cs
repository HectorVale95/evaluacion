using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ClbModEvaluacion;
using ClbNegEvaluacion;

namespace WebEvaluacion.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly ClsNegEmpleados _negEmpleados;
        private readonly string _connectionString;

        public EmpleadosController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _negEmpleados = new ClsNegEmpleados(_connectionString);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClsModEmpleado>> GetAll()
        {
            try
            {
                return Ok(_negEmpleados.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los empleados: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ClsModEmpleado> GetById(int id)
        {
            try
            {
                var empleado = _negEmpleados.GetById(id);
                if (empleado == null)
                    return NotFound($"No se encontró el empleado con ID {id}");

                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el empleado: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<int> Create([FromBody] ClsModEmpleado empleado)
        {
            try
            {
                var id = _negEmpleados.Create(empleado);
                return CreatedAtAction(nameof(GetById), new { id = id }, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el empleado: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClsModEmpleado empleado)
        {
            try
            {
                if (id != empleado.IdEmpleado)
                    return BadRequest("El ID del empleado no coincide");

                _negEmpleados.Update(empleado);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el empleado: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _negEmpleados.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el empleado: {ex.Message}");
            }
        }
    }
} 