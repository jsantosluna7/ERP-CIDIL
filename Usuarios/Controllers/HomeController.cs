using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Servicios; // ✅ Namespace correcto
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnuncioController : ControllerBase
    {
        private readonly IAnuncioServicio _anuncioServicio;

        public AnuncioController(IAnuncioServicio anuncioServicio)
        {
            _anuncioServicio = anuncioServicio ?? throw new ArgumentNullException(nameof(anuncioServicio));
        }

        // GET: api/anuncio
        [HttpGet]
        public async Task<IActionResult> ObtenerAnuncios()
        {
            var anuncios = await _anuncioServicio.ObtenerTodosAsync();
            return Ok(anuncios);
        }

        // POST: api/anuncio
        [HttpPost]
        public async Task<IActionResult> CrearAnuncio([FromForm] CrearAnuncioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo) || string.IsNullOrWhiteSpace(dto.Descripcion))
                return BadRequest(new { error = "El título y la descripción son obligatorios." });

            if (dto.Imagen == null || dto.Imagen.Length == 0)
                return BadRequest(new { error = "Debe proporcionar una imagen para el anuncio." });

            var extension = Path.GetExtension(dto.Imagen.FileName).ToLower();
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                return BadRequest(new { error = "Solo se permiten imágenes en formato JPG, JPEG o PNG." });

            if (dto.Imagen.Length > 3 * 1024 * 1024)
                return BadRequest(new { error = "La imagen es demasiado grande (máx. 3 MB)." });

            try
            {
                // Crear carpeta si no existe
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "anuncios");
                Directory.CreateDirectory(carpeta);

                // Guardar imagen con nombre único
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await dto.Imagen.CopyToAsync(stream);
                }

                var urlImagen = $"/imagenes/anuncios/{nombreArchivo}";

                // Crear DTO para servicio (solo con datos necesarios)
                var crearDto = new CrearAnuncioDTO
                {
                    Titulo = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    Imagen = dto.Imagen
                };

                // Guardar en la base de datos mediante el servicio
                await _anuncioServicio.CrearAsync(crearDto);

                return Ok(new
                {
                    mensaje = "Anuncio creado correctamente.",
                    titulo = dto.Titulo,
                    descripcion = dto.Descripcion,
                    imagen = urlImagen
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al crear el anuncio.", detalle = ex.Message });
            }
        }

        // PUT: api/anuncio/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAnuncio(int id, [FromBody] ActualizarAnuncioDTO dto)
        {
            var actualizado = await _anuncioServicio.ActualizarAsync(id, dto);
            if (!actualizado)
                return NotFound(new { error = $"No se encontró el anuncio con ID {id}." });

            return Ok(new { mensaje = "Anuncio actualizado correctamente." });
        }

        // DELETE: api/anuncio/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAnuncio(int id)
        {
            var eliminado = await _anuncioServicio.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { error = $"No se encontró el anuncio con ID {id}." });

            return Ok(new { mensaje = "Anuncio eliminado correctamente." });
        }
    }
}
