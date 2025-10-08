using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly ILikeServicio _likeServicio;

        public LikeController(ILikeServicio likeServicio)
        {
            _likeServicio = likeServicio;
        }

        // 🔹 GET: api/like
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var lista = await _likeServicio.ObtenerTodosAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error: _logger.LogError(ex, "Error obteniendo likes");
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener los likes" });
            }
        }

        // 🔹 GET: api/like/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var like = await _likeServicio.ObtenerPorIdAsync(id);
                if (like == null)
                    return NotFound(new { mensaje = "Like no encontrado" });

                return Ok(like);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al obtener el like" });
            }
        }

        // 🔹 POST: api/like
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] LikeDTO dto)
        {
            if (dto == null)
                return BadRequest(new { mensaje = "Datos inválidos" });

            try
            {
                var creado = await _likeServicio.CrearAsync(dto);
                if (!creado)
                    return BadRequest(new { mensaje = "No se pudo crear el like (posiblemente el anuncio no existe o ya fue likeado)" });

                return Ok(new { mensaje = "Like creado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al crear el like" });
            }
        }

        // 🔹 DELETE: api/like/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var eliminado = await _likeServicio.EliminarAsync(id);
                if (!eliminado)
                    return NotFound(new { mensaje = "Like no encontrado para eliminar" });

                return Ok(new { mensaje = "Like eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al eliminar el like" });
            }
        }

        // 🔹 GET: api/like/contar/{anuncioId}
        [HttpGet("contar/{anuncioId}")]
        public async Task<IActionResult> ContarPorAnuncio(int anuncioId)
        {
            try
            {
                var cantidad = await _likeServicio.ContarPorAnuncioAsync(anuncioId);
                return Ok(new
                {
                    anuncioId,
                    cantidadLikes = cantidad
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al contar los likes" });
            }
        }
    }
}
