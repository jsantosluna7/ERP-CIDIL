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

        // ✅ GET: Lista todos los currículos (solo SUPERUSUARIO o ADMINISTRADOR)
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized(new { mensaje = "No tienes permiso para acceder a esta información" });

            var lista = await _curriculumServicio.ObtenerTodosAsync();
            return Ok(lista);
        }

        // ✅ GET: Obtiene un currículum por Id (solo SUPERUSUARIO o ADMINISTRADOR)
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized(new { mensaje = "No tienes permiso para acceder a esta información" });

            var item = await _curriculumServicio.ObtenerPorIdAsync(id);
            if (item == null)
                return NotFound(new { mensaje = "Currículum no encontrado" });

            return Ok(item);
        }

        // ✅ POST: Cualquier usuario puede enviar su currículum
        // (incluye estudiantes, profesores, administradores o superusuarios)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] CurriculumDTO dto)
        {
            try
            {
                if (!User.TieneRol("1", "2", "3", "4"))
                    return Unauthorized(new { mensaje = "No tienes permiso para realizar esta acción" });

                await _curriculumServicio.CrearAsync(dto);
                return Ok(new { mensaje = "Currículum enviado correctamente ✅" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al subir el currículum. Intenta nuevamente." });
            }
        }

        // ✅ DELETE: Elimina un currículum (solo SUPERUSUARIO o ADMINISTRADOR)
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized(new { mensaje = "No tienes permiso para eliminar currículos" });

            var eliminado = await _curriculumServicio.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Currículum no encontrado para eliminar" });

            return Ok(new { mensaje = "Currículum eliminado correctamente ✅" });
        }
    }
}
