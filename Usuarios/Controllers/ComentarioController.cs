using ERP.Data.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Controllers      
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔒 Requiere autenticación por defecto
    public class ComentarioController : ControllerBase
    {
        private readonly IServicioComentario _comentarioServicio;

        public ComentarioController(IServicioComentario comentarioServicio)
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
        [AllowAnonymous] //Permite ver comentarios sin iniciar sesión
        public async Task<IActionResult> ObtenerComentariosPorAnuncio(int anuncioId)
        {
            var comentarios = await _comentarioServicio.ObtenerPorAnuncioIdAsync(anuncioId);
            return Ok(comentarios);
        }

        // ==================== Crear comentario (solo PROFESOR o ESTUDIANTE) ====================
        [HttpPost]
        public async Task<IActionResult> CrearComentario([FromBody] CrearComentarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔹 Validar rol (solo profesor o estudiante)
            if (!User.TieneRol("3", "4")) // 3 = Profesor, 4 = Estudiante
                return Forbid("No tienes permisos para comentar. Solo profesores o estudiantes pueden hacerlo.");

            var resultado = await _comentarioServicio.CrearAsync(dto);

            if (!resultado.esExitoso)
                return StatusCode(500, new { error = resultado.MensajeError });

            return Ok(new
            {
                mensaje = "Comentario creado correctamente ✅",
                comentario = resultado.Valor
            });
        }

        // ==================== Actualizar comentario (solo SUPERUSUARIO o ADMINISTRADOR) ====================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarComentario(int id, [FromBody] ActualizarComentarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔹 Solo superusuario o administrador pueden actualizar
            if (!User.TieneRol("1", "2")) // 1 = Superusuario, 2 = Administrador
                return Forbid("No tienes permisos para actualizar comentarios.");

            var resultado = await _comentarioServicio.ActualizarAsync(id, dto);

            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(new { mensaje = "Comentario actualizado correctamente ✅" });
        }

        // ==================== Eliminar comentario (solo SUPERUSUARIO o ADMINISTRADOR) ====================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarComentario(int id)
        {
            // 🔹 Solo superusuario o administrador pueden eliminar
            if (!User.TieneRol("1", "2"))
                return Forbid("No tienes permisos para eliminar comentarios.");

            var resultado = await _comentarioServicio.EliminarAsync(id);

            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(new { mensaje = "Comentario eliminado correctamente ✅" });
        }
    }
}
