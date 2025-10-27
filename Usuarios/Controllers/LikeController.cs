using ERP.Data;
using ERP.Data.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
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

        // ==================== Dar/Quitar Like ====================
        [HttpPost]
        public async Task<IActionResult> ToggleLike([FromBody] LikeDTO dto)
        {
            // Solo estudiantes y profesores pueden dar likes
            if (!User.TieneRol("ESTUDIANTE", "PROFESOR"))
                return Unauthorized("No tienes permisos para dar likes.");

            if (dto == null || dto.AnuncioId <= 0 || string.IsNullOrEmpty(dto.Usuario))
                return BadRequest(new { mensaje = "Datos inválidos para like." });

            try
            {
                // Buscar usuario por correo institucional
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.CorreoInstitucional == dto.Usuario);

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
                    totalLikes
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al procesar el like.", detalle = ex.Message });
            }
        }

        // ==================== Contar likes de un anuncio ====================
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

        // ==================== Verificar si un usuario ya dio like ====================
        [HttpGet("existe/{anuncioId}/{correoInstitucional}")]
        public async Task<IActionResult> ExisteLike(int anuncioId, string correoInstitucional)
        {
            if (anuncioId <= 0 || string.IsNullOrEmpty(correoInstitucional))
                return BadRequest(new { mensaje = "Datos inválidos." });

            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.CorreoInstitucional == correoInstitucional);

                if (usuario == null)
                    return BadRequest(new { mensaje = "Usuario no encontrado." });

                bool existe = await _context.Likes
                    .AnyAsync(l => l.AnuncioId == anuncioId && l.UsuarioId == usuario.Id);

                return Ok(new { anuncioId, usuario = correoInstitucional, existe });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al verificar el like.", detalle = ex.Message });
            }
        }
    }
}
