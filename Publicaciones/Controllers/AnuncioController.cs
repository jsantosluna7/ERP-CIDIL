using ERP.Data.Modelos;
using Microsoft.AspNetCore.Mvc;
using Publicaciones.Abstraccion.Servicios;
using Publicaciones.DTO.AnuncioDTO;

namespace Publicaciones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnuncioController : ControllerBase
    {
        private readonly IServicioAnuncio _servicioAnuncio;

        public AnuncioController(IServicioAnuncio servicioAnuncio)
        {
            _servicioAnuncio = servicioAnuncio;
        }

        // PÚBLICO: Obtener todos los anuncios
        [HttpGet]
        public async Task<IActionResult> GetAnuncios()
        {
            var resultado = await _servicioAnuncio.GetAnuncios();
            if (resultado == null)
                return NotFound("No se encontraron anuncios.");
            return Ok(resultado);
        }

        // PÚBLICO: Obtener anuncios para el carrusel destacado
        [HttpGet("carrusel")]
        public async Task<IActionResult> GetCarrusel()
        {
            var resultado = await _servicioAnuncio.GetCarrusel();
            if (resultado == null)
                return NotFound("No hay anuncios en el carrusel.");
            return Ok(resultado);
        }

        // PÚBLICO: Obtener solo pasantías
        [HttpGet("pasantias")]
        public async Task<IActionResult> GetPasantias()
        {
            var resultado = await _servicioAnuncio.GetPasantias();
            if (resultado == null)
                return NotFound("No hay pasantías disponibles.");
            return Ok(resultado);
        }

        // PÚBLICO: Obtener un anuncio por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultado = await _servicioAnuncio.GetById(id);
            if (resultado == null)
                return NotFound($"No se encontró el anuncio con ID: {id}.");
            return Ok(resultado);
        }

        // PROTEGIDO: Solo Administrador y Supervisor pueden crear anuncios
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearAnuncioDTO dto)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized("No tienes permiso para crear anuncios.");

            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (!int.TryParse(idClaim, out int usuarioId))
                return Unauthorized("No se pudo identificar al usuario.");

            var resultado = await _servicioAnuncio.Crear(dto, usuarioId);
            if (resultado == null)
                return BadRequest("No se pudo crear el anuncio.");
            return Ok(resultado);
        }

        // PROTEGIDO: Solo Administrador y Supervisor pueden actualizar anuncios
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarAnuncioDTO dto)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized("No tienes permiso para actualizar anuncios.");

            var resultado = await _servicioAnuncio.Actualizar(id, dto);
            if (resultado == null)
                return NotFound($"No se encontró el anuncio con ID: {id}.");
            return Ok(resultado);
        }

        // PROTEGIDO: Solo Administrador puede eliminar anuncios
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (!User.TieneRol("1"))
                return Unauthorized("No tienes permiso para eliminar anuncios.");

            var resultado = await _servicioAnuncio.Eliminar(id);
            if (resultado == null)
                return NotFound($"No se encontró el anuncio con ID: {id}.");
            return Ok(new { mensaje = "Anuncio eliminado correctamente." });
        }
    }
}
