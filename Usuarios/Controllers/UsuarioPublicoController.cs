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

        // ✅ DTO para registrar o iniciar sesión
        public class UsuarioPublicoDTO
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            public string Nombre { get; set; } = string.Empty;

            [Required(ErrorMessage = "El correo es obligatorio.")]
            [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
            public string Correo { get; set; } = string.Empty;
        }

        // ✅ REGISTRAR USUARIO
        // POST: api/UsuarioPublico/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioPublicoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar si ya existe un usuario con el mismo correo
            var existente = await _context.UsuarioPublicos
                .FirstOrDefaultAsync(u => u.Correo.ToLower() == dto.Correo.ToLower());

            if (existente != null)
                return Conflict(new { mensaje = $"El correo {dto.Correo} ya está registrado." });

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

        // ✅ LOGIN USUARIO
        // POST: api/UsuarioPublico/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioPublicoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Buscar usuario por correo
            var usuario = await _context.UsuarioPublicos
                .FirstOrDefaultAsync(u => u.Correo.ToLower() == dto.Correo.ToLower());

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado. Por favor, regístrate primero." });
            }

            // Validar nombre (opcional, pero recomendable)
            if (!string.Equals(usuario.Nombre, dto.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { mensaje = "El nombre no coincide con el correo." });
            }

            // Retornar usuario existente
            return Ok(new
            {
                mensaje = "Inicio de sesión exitoso.",
                usuario.Id,
                usuario.Nombre,
                usuario.Correo
            });
        }

        // ✅ OBTENER USUARIO POR ID
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
