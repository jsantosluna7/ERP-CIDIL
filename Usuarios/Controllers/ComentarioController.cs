using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;
using ERP.Data.Modelos;

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

        // ==================== Obtener todos los comentarios ====================
        [HttpGet]
        public async Task<IActionResult> ObtenerComentarios()
        {
            var comentarios = await _comentarioServicio.ObtenerTodosAsync();
            return Ok(comentarios);
        }

        // ==================== Obtener comentarios por anuncio ====================
        [HttpGet("anuncio/{anuncioId}")]
        public async Task<IActionResult> ObtenerComentariosPorAnuncio(int anuncioId)
        {
            var comentarios = await _comentarioServicio.ObtenerPorAnuncioIdAsync(anuncioId);
            return Ok(comentarios);
        }

        // ==================== Crear comentario (Solo ESTUDIANTE/PROFESOR) ====================
        [HttpPost]
        public async Task<IActionResult> CrearComentario([FromBody] CrearComentarioDTO dto)
        {
            if (!User.TieneRol("ESTUDIANTE", "PROFESOR"))
                return Unauthorized("No tienes permisos para crear comentarios.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _comentarioServicio.CrearAsync(dto);

                if (!resultado.esExitoso)
                    return StatusCode(500, new { error = resultado.MensajeError });

                return Ok(resultado.Valor);
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

        // ==================== Actualizar comentario (Solo ADMINISTRADOR/SUPERUSUARIO) ====================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarComentario(int id, [FromBody] ActualizarComentarioDTO dto)
        {
            if (!User.TieneRol("ADMINISTRADOR", "SUPERUSUARIO"))
                return Unauthorized("No tienes permisos para actualizar comentarios.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _comentarioServicio.ActualizarAsync(id, dto);

            if (!resultado.esExitoso)
                return NotFound(resultado.MensajeError);

            return Ok("Comentario actualizado correctamente.");
        }

        // ==================== Eliminar comentario (Solo ADMINISTRADOR/SUPERUSUARIO) ====================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarComentario(int id)
        {
            if (!User.TieneRol("ADMINISTRADOR", "SUPERUSUARIO"))
                return Unauthorized("No tienes permisos para eliminar comentarios.");

            var resultado = await _comentarioServicio.EliminarAsync(id);

            if (!resultado.esExitoso)
                return NotFound(resultado.MensajeError);

            return Ok("Comentario eliminado correctamente.");
        }
    }
}
