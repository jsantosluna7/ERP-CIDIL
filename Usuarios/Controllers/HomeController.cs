using Microsoft.AspNetCore.Mvc;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.Implementaciones.Servicios;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnuncioController : ControllerBase
    {
        private readonly IAnuncioServicio _anuncioServicio;

        public AnuncioController(IAnuncioServicio anuncioServicio)
        {
            _anuncioServicio = anuncioServicio;
        }

        // ✅ GET: api/anuncio
        [HttpGet]
        public async Task<IActionResult> ObtenerAnuncios()
        {
            var anuncios = await _anuncioServicio.ObtenerTodosAsync();
            return Ok(anuncios);
        }

        // ✅ POST: api/anuncio
        [HttpPost]
        public async Task<IActionResult> CrearAnuncio([FromBody] CrearAnuncioDTO dto)
        {
            await _anuncioServicio.CrearAsync(dto);
            return Ok("Anuncio creado correctamente.");
        }

        // ✅ PUT: api/anuncio/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAnuncio(int id, [FromBody] ActualizarAnuncioDTO dto)
        {
            var actualizado = await _anuncioServicio.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound($"No se encontró el anuncio con ID {id}");

            return Ok("Anuncio actualizado correctamente.");
        }

        // ✅ DELETE: api/anuncio/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAnuncio(int id)
        {
            var eliminado = await _anuncioServicio.EliminarAsync(id);

            if (!eliminado)
                return NotFound($"No se encontró el anuncio con ID {id}");

            return Ok("Anuncio eliminado correctamente.");
        }

        // 🔹 POST: api/anuncio/subir-curriculum
        [HttpPost("subir-curriculum")]
        public async Task<IActionResult> SubirCurriculum(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("No se seleccionó ningún archivo.");

            var extension = Path.GetExtension(archivo.FileName).ToLower();
            if (extension != ".pdf")
                return BadRequest("Solo se permiten archivos PDF.");

            if (archivo.Length > 5 * 1024 * 1024) // 5 MB máximo
                return BadRequest("El archivo es demasiado grande.");

            var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Curriculums");
            Directory.CreateDirectory(carpeta); // Crear carpeta si no existe

            var ruta = Path.Combine(carpeta, archivo.FileName);

            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            return Ok("Currículum subido correctamente.");
        }
    }
}
