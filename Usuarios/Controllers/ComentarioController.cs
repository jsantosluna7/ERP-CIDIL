using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioServicio _comentarioServicio;

        public ComentarioController(IComentarioServicio comentarioServicio)
        {
            _comentarioServicio = comentarioServicio;
        }

        // 🔹 Obtener todos los comentarios
        [HttpGet]
        public async Task<IActionResult> ObtenerComentarios()
        {
            var comentarios = await _comentarioServicio.ObtenerTodosAsync();
            return Ok(comentarios);
        }

        // 🔹 Obtener comentarios por anuncio
        [HttpGet("anuncio/{anuncioId}")]
        public async Task<IActionResult> ObtenerComentariosPorAnuncio(int anuncioId)
        {
            var comentarios = await _comentarioServicio.ObtenerPorAnuncioIdAsync(anuncioId);
            return Ok(comentarios);
        }

        // 🔹 Crear comentario (devuelve el creado con ID)
        [HttpPost]
        public async Task<IActionResult> CrearComentario([FromBody] CrearComentarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var nuevoComentario = await _comentarioServicio.CrearAsync(dto); // ✅ ya devuelve un objeto
                return Ok(nuevoComentario); // ✅ sin error de tipo
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al crear el comentario.", detalle = ex.Message });
            }
        }

        // 🔹 Actualizar comentario
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarComentario(int id, [FromBody] ActualizarComentarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exito = await _comentarioServicio.ActualizarAsync(id, dto);
            if (!exito)
                return NotFound("No se encontró el comentario o el texto es inválido.");

            return Ok("Comentario actualizado correctamente.");
        }

        // 🔹 Eliminar comentario
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarComentario(int id)
        {
            var exito = await _comentarioServicio.EliminarAsync(id);
            if (!exito)
                return NotFound("Comentario no encontrado para eliminar.");

            return Ok("Comentario eliminado correctamente.");
        }
    }
}
