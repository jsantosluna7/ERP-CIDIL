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

        [HttpGet]
        public async Task<IActionResult> ObtenerComentarios()
        {
            var comentarios = await _comentarioServicio.ObtenerTodosAsync();
            return Ok(comentarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerComentarioPorId(int id)
        {
            var comentario = await _comentarioServicio.ObtenerPorIdAsync(id);
            if (comentario == null)
                return NotFound("Comentario no encontrado.");
            return Ok(comentario);
        }

        [HttpPost]
        public async Task<IActionResult> CrearComentario([FromBody] CrearComentarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _comentarioServicio.CrearAsync(dto);
                return Ok("Comentario creado correctamente.");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error al crear el comentario.");
            }
        }

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
