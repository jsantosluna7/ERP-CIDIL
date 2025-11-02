using Microsoft.AspNetCore.Authorization;
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

        // ==================== Crear comentario (Solo PROFESOR o ESTUDIANTE) ====================
        [HttpPost]
        [Authorize(Roles = "3,4")] // Profesor(3) o Estudiante(4)
        public async Task<IActionResult> CrearComentario([FromBody] CrearComentarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _comentarioServicio.CrearAsync(dto);

                if (!resultado.esExitoso)
                    return StatusCode(500, new { error = resultado.MensajeError });

                return Ok(new
                {
                    mensaje = "Comentario creado correctamente ✅",
                    comentario = resultado.Valor
                });
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
                return StatusCode(500, new
                {
                    error = "Ocurrió un error al crear el comentario.",
                    detalle = ex.Message
                });
            }
        }

        // ==================== Actualizar comentario (Solo SUPERUSUARIO o ADMINISTRADOR) ====================
        [HttpPut("{id}")]
        [Authorize(Roles = "1,2")] // Superusuario(1) o Administrador(2)
        public async Task<IActionResult> ActualizarComentario(int id, [FromBody] ActualizarComentarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _comentarioServicio.ActualizarAsync(id, dto);

            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(new { mensaje = "Comentario actualizado correctamente ✅" });
        }

        // ==================== Eliminar comentario (Solo SUPERUSUARIO o ADMINISTRADOR) ====================
        [HttpDelete("{id}")]
        [Authorize(Roles = "1,2")] // Superusuario(1) o Administrador(2)
        public async Task<IActionResult> EliminarComentario(int id)
        {
            var resultado = await _comentarioServicio.EliminarAsync(id);

            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(new { mensaje = "Comentario eliminado correctamente ✅" });
        }
    }
}
