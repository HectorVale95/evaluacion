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
    public class PuestosController : ControllerBase
    {
        private readonly ClsNegPuestos _negPuestos;
        private readonly string _connectionString;

        public PuestosController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _negPuestos = new ClsNegPuestos(_connectionString);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClsModPuesto>> GetAll()
        {
            try
            {
                return Ok(_negPuestos.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los puestos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ClsModPuesto> GetById(int id)
        {
            try
            {
                var puesto = _negPuestos.GetById(id);
                if (puesto == null)
                    return NotFound($"No se encontró el puesto con ID {id}");

                return Ok(puesto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el puesto: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<int> Create([FromBody] ClsModPuesto puesto)
        {
            try
            {
                var id = _negPuestos.Create(puesto);
                return CreatedAtAction(nameof(GetById), new { id = id }, id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el puesto: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClsModPuesto puesto)
        {
            try
            {
                if (id != puesto.IdPuesto)
                    return BadRequest("El ID del puesto no coincide");

                _negPuestos.Update(puesto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el puesto: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _negPuestos.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el puesto: {ex.Message}");
            }
        }
    }
} 