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

        /// <summary>
        /// Alterna el estado de un like (añadir o quitar)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ToggleLike([FromBody] LikeDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Usuario) || dto.AnuncioId <= 0)
                return BadRequest(new { mensaje = "Datos inválidos para like." });

            try
            {
                var resultado = await _likeServicio.CrearAsync(dto);

                return Ok(new
                {
                    mensaje = resultado.estadoActual ? "Like añadido" : "Like quitado",
                    anuncioId = dto.AnuncioId,
                    estado = resultado.estadoActual,
                    totalLikes = resultado.totalLikes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al procesar el like.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la cantidad total de likes de un anuncio
        /// </summary>
        [HttpGet("contar/{anuncioId}")]
        public async Task<IActionResult> ContarPorAnuncio(int anuncioId)
        {
            if (anuncioId <= 0)
                return BadRequest(new { mensaje = "ID de anuncio inválido." });

            try
            {
                int total = await _likeServicio.ContarPorAnuncioAsync(anuncioId);
                return Ok(new { anuncioId, totalLikes = total });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al contar likes.", detalle = ex.Message });
            }
        }

        /// <summary>
        /// ✅ Verifica si el usuario ya dio like a un anuncio
        /// </summary>
        [HttpGet("existe/{anuncioId}/{usuario}")]
        public async Task<IActionResult> ExisteLike(int anuncioId, string usuario)
        {
            if (anuncioId <= 0 || string.IsNullOrWhiteSpace(usuario))
                return BadRequest(new { mensaje = "Datos inválidos." });

            try
            {
                bool existe = await _likeServicio.ExisteLikeAsync(anuncioId, usuario);
                return Ok(new { anuncioId, usuario, existe });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al verificar el like.", detalle = ex.Message });
            }
        }
    }
}
