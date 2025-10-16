using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ERP.Data.Modelos;
using ERP.Data;
using Microsoft.EntityFrameworkCore;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly DbErpContext _context;

        public LikeController(DbErpContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike([FromBody] LikeDTO dto)
        {
            if (dto == null || dto.AnuncioId <= 0 || (dto.UsuarioId == null && string.IsNullOrEmpty(dto.Usuario)))
                return BadRequest(new { mensaje = "Datos inválidos para like." });

            try
            {
                UsuarioPublico? usuario;

                // Buscar usuario por Id o por nombre/correo
                if (dto.UsuarioId != null)
                {
                    usuario = await _context.UsuarioPublicos.FindAsync(dto.UsuarioId.Value);
                }
                else
                {
                    usuario = await _context.UsuarioPublicos
                        .FirstOrDefaultAsync(u => u.Nombre == dto.Usuario || u.Correo == dto.Usuario);
                }

                if (usuario == null)
                    return BadRequest(new { mensaje = "Usuario no encontrado." });

                // Buscar si ya existe el like
                var likeExistente = await _context.Likes
                    .FirstOrDefaultAsync(l => l.AnuncioId == dto.AnuncioId && l.UsuarioId == usuario.Id);

                bool estadoActual;

                if (likeExistente != null)
                {
                    // Quitar like existente
                    _context.Likes.Remove(likeExistente);
                    estadoActual = false;
                }
                else
                {
                    // Agregar nuevo like
                    var nuevoLike = new Like
                    {
                        AnuncioId = dto.AnuncioId,
                        UsuarioId = usuario.Id,
                        Fecha = DateTime.UtcNow
                    };
                    _context.Likes.Add(nuevoLike);
                    estadoActual = true;
                }

                await _context.SaveChangesAsync();

                // Contar likes actuales del anuncio
                int totalLikes = await _context.Likes.CountAsync(l => l.AnuncioId == dto.AnuncioId);

                return Ok(new
                {
                    mensaje = estadoActual ? "Like añadido" : "Like quitado",
                    anuncioId = dto.AnuncioId,
                    estado = estadoActual,
                    totalLikes = totalLikes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al procesar el like.", detalle = ex.Message });
            }
        }

        [HttpGet("contar/{anuncioId}")]
        public async Task<IActionResult> ContarPorAnuncio(int anuncioId)
        {
            if (anuncioId <= 0)
                return BadRequest(new { mensaje = "ID de anuncio inválido." });

            try
            {
                int total = await _context.Likes.CountAsync(l => l.AnuncioId == anuncioId);
                return Ok(new { anuncioId, totalLikes = total });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al contar likes.", detalle = ex.Message });
            }
        }

        [HttpGet("existe/{anuncioId}/{usuario}")]
        public async Task<IActionResult> ExisteLike(int anuncioId, string usuario)
        {
            if (anuncioId <= 0 || string.IsNullOrEmpty(usuario))
                return BadRequest(new { mensaje = "Datos inválidos." });

            try
            {
                var usuarioEntity = await _context.UsuarioPublicos
                    .FirstOrDefaultAsync(u => u.Nombre == usuario || u.Correo == usuario);

                if (usuarioEntity == null)
                    return BadRequest(new { mensaje = "Usuario no encontrado." });

                bool existe = await _context.Likes
                    .AnyAsync(l => l.AnuncioId == anuncioId && l.UsuarioId == usuarioEntity.Id);

                return Ok(new { anuncioId, usuario, existe });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al verificar el like.", detalle = ex.Message });
            }
        }
    }
}
