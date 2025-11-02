using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ERP.Data.Modelos; // 👈 Para usar User.TieneRol()
using System;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurriculumController : ControllerBase
    {
        private readonly ICurriculumServicio _curriculumServicio;

        public CurriculumController(ICurriculumServicio curriculumServicio)
        {
            _curriculumServicio = curriculumServicio;
        }

        // ==================== GET: Lista todos los currículos ====================
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized(new { mensaje = "No tienes permiso para acceder a esta información" });

            var resultado = await _curriculumServicio.ObtenerTodosAsync();
            if (!resultado.esExitoso)
                return StatusCode(500, new { mensaje = resultado.MensajeError });

            return Ok(resultado.Valor);
        }

        // ==================== GET: Obtiene un currículum por Id ====================
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized(new { mensaje = "No tienes permiso para acceder a esta información" });

            var resultado = await _curriculumServicio.ObtenerPorIdAsync(id);
            if (!resultado.esExitoso || resultado.Valor == null)
                return NotFound(new { mensaje = "Currículum no encontrado" });

            return Ok(resultado.Valor);
        }

        // ==================== POST: Crear currículum ====================
        [AllowAnonymous] // ✅ Permite que los externos suban sin iniciar sesión
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] CurriculumDTO dto)
        {
            // Validar que se envíen los datos mínimos
            if (dto == null)
                return BadRequest(new { mensaje = "No se recibieron los datos del currículum." });

            try
            {
                // Si el usuario está autenticado, validar roles
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    if (!User.TieneRol("1", "2", "3", "4"))
                        return Unauthorized(new { mensaje = "No tienes permiso para realizar esta acción" });

                    // 🔹 Crear como usuario autenticado
                    var resultado = await _curriculumServicio.CrearAsync(dto);
                    if (!resultado.esExitoso)
                        return BadRequest(new { mensaje = resultado.MensajeError });

                    return Ok(new { mensaje = "Currículum registrado correctamente ✅" });
                }
                else
                {
                    // 🔹 Crear como usuario externo (sin login)
                    var resultado = await _curriculumServicio.CrearExternoAsync(dto);
                    if (!resultado.esExitoso)
                        return BadRequest(new { mensaje = resultado.MensajeError });

                    return Ok(new { mensaje = "Currículum enviado correctamente ✅" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = $"Error interno al subir el currículum: {ex.Message}" });
            }
        }

        // ==================== DELETE: Eliminar currículum ====================
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized(new { mensaje = "No tienes permiso para eliminar currículos" });

            var resultado = await _curriculumServicio.EliminarAsync(id);
            if (!resultado.esExitoso || !resultado.Valor)
                return NotFound(new { mensaje = resultado.MensajeError ?? "Currículum no encontrado para eliminar" });

            return Ok(new { mensaje = "Currículum eliminado correctamente ✅" });
        }
    }
}
