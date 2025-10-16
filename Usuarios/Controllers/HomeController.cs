using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO;
using ERP.Data.Modelos;

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

        // ✅ Obtener todos los anuncios con comentarios, likes y usuario
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerAnuncios([FromQuery] bool? esPasantia)
        {
            var anuncios = await _anuncioServicio.ObtenerTodosAsync(esPasantia);

            if (anuncios == null || !anuncios.Any())
                return Ok(Array.Empty<object>());

            return Ok(anuncios);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CrearAnuncio([FromForm] CrearAnuncioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var extension = Path.GetExtension(dto.Imagen.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg")
                    return BadRequest(new { error = "Solo se permiten imágenes JPG o JPEG." });

                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "anuncios");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    await dto.Imagen.CopyToAsync(stream);

                var anuncio = new Anuncio
                {
                    Titulo = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    ImagenUrl = $"/imagenes/anuncios/{nombreArchivo}",
                    EsPasantia = dto.EsPasantia,
                    FechaPublicacion = DateTime.Now
                };

                await _anuncioServicio.CrearAsync(anuncio);
                return Ok(new { mensaje = "Anuncio creado correctamente.", anuncio });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al crear el anuncio.", detalle = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAnuncio(int id, [FromForm] ActualizarAnuncioDTO dto)
        {
            if (dto == null)
                return BadRequest(new { error = "Los datos del anuncio no pueden estar vacíos." });

            var anuncioExistente = await _anuncioServicio.ObtenerPorIdAsync(id);
            if (anuncioExistente == null)
                return NotFound(new { error = $"No se encontró el anuncio con ID {id}." });

            string nuevaUrlImagen = anuncioExistente.ImagenUrl;

            if (dto.Imagen != null && dto.Imagen.Length > 0)
            {
                var extension = Path.GetExtension(dto.Imagen.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg")
                    return BadRequest(new { error = "Solo se permiten imágenes JPG o JPEG." });

                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "anuncios");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    await dto.Imagen.CopyToAsync(stream);

                nuevaUrlImagen = $"/imagenes/anuncios/{nombreArchivo}";
            }

            dto.ImagenUrl = nuevaUrlImagen;
            var actualizado = await _anuncioServicio.ActualizarAsync(id, dto);

            if (!actualizado)
                return StatusCode(500, new { error = "No se pudo actualizar el anuncio." });

            return Ok(new { mensaje = "Anuncio actualizado correctamente.", anuncio = dto });
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAnuncio(int id)
        {
            var eliminado = await _anuncioServicio.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { error = $"No se encontró el anuncio con ID {id}." });

            return Ok(new { mensaje = "Anuncio eliminado correctamente." });
        }

        [AllowAnonymous]
        [HttpGet("{id}/curriculums")]
        public async Task<IActionResult> VerCurriculums(int id)
        {
            var anuncio = await _anuncioServicio.ObtenerPorIdAsync(id);
            if (anuncio == null)
                return NotFound(new { error = "Anuncio no encontrado." });

            if (!anuncio.EsPasantia)
                return Ok(new { mensaje = "Este anuncio no es de pasantía.", curriculums = Array.Empty<object>() });

            var curriculums = await _anuncioServicio.ObtenerCurriculumsAsync(id);
            return Ok(curriculums);
        }
    }
}
