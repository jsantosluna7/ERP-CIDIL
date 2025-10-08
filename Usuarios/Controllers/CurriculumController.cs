using Microsoft.AspNetCore.Mvc;
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

        // ✅ GET: api/curriculum
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var lista = await _curriculumServicio.ObtenerTodosAsync();
            return Ok(lista);
        }

        // ✅ GET: api/curriculum/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var item = await _curriculumServicio.ObtenerPorIdAsync(id);
            if (item == null)
                return NotFound(new { mensaje = "Currículum no encontrado" });

            return Ok(item);
        }

        // ✅ POST: api/curriculum
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] CurriculumDTO dto)
        {
            try
            {
                await _curriculumServicio.CrearAsync(dto);
                return Ok(new { mensaje = "Currículum enviado correctamente ✅" });
            }
            catch (InvalidOperationException ex)
            {
                // ⚠️ Error controlado (por ejemplo: archivo no PDF)
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                // ⚠️ Error inesperado del servidor
                return StatusCode(500, new { mensaje = "Ocurrió un error al subir el currículum. Intenta nuevamente." });
            }
        }

        // ✅ DELETE: api/curriculum/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _curriculumServicio.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = "Currículum no encontrado para eliminar" });

            return Ok(new { mensaje = "Currículum eliminado correctamente ✅" });
        }
    }
}
