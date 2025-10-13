using Microsoft.AspNetCore.Mvc;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioPublicoController : ControllerBase
    {
        private readonly DbErpContext _context;

        public UsuarioPublicoController(DbErpContext context)
        {
            _context = context;
        }

        // DTO para registrar un nuevo usuario público
        public class CrearUsuarioPublicoDTO
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            public string Nombre { get; set; } = string.Empty;

            [Required(ErrorMessage = "El correo es obligatorio.")]
            [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
            public string Correo { get; set; } = string.Empty;
        }

        // POST: api/UsuarioPublico/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] CrearUsuarioPublicoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar si ya existe un usuario con el mismo correo
            var existente = await _context.UsuarioPublicos
                .FirstOrDefaultAsync(u => u.Correo == dto.Correo);

            if (existente != null)
                return BadRequest($"Ya existe un usuario registrado con el correo {dto.Correo}.");

            var usuario = new UsuarioPublico
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                FechaRegistro = DateTime.UtcNow
            };

            _context.UsuarioPublicos.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Usuario registrado correctamente.",
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                usuario.FechaRegistro
            });
        }

        // GET: api/UsuarioPublico/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var usuario = await _context.UsuarioPublicos
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Ok(usuario);
        }
    }
}
