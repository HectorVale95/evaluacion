using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ClbModEvaluacion;
using ClbNegEvaluacion;
using System;

namespace WebEvaluacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ClsNegAuth _negAuth;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _negAuth = new ClsNegAuth(configuration);
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] ClsModAuth auth)
        {
            try
            {
                if (auth == null || string.IsNullOrEmpty(auth.Username) || string.IsNullOrEmpty(auth.Password))
                {
                    return BadRequest(new { message = "Usuario y contraseña son requeridos" });
                }

                _logger.LogInformation($"Intento de login para el usuario: {auth.Username}");
                var response = _negAuth.Authenticate(auth);
                _logger.LogInformation($"Login exitoso para el usuario: {auth.Username}");
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Login fallido para el usuario: {auth?.Username}. Razón: {ex.Message}");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante el login para el usuario: {auth?.Username}");
                return StatusCode(500, new { 
                    message = "Error interno del servidor", 
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
