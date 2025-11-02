using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        // ==================== Obtener todos los anuncios ====================
        [HttpGet]
        public async Task<IActionResult> ObtenerAnuncios([FromQuery] bool? esPasantia)
        {
            var resultado = await _anuncioServicio.ObtenerTodosAsync(esPasantia);

            if (!resultado.esExitoso || resultado.Valor == null)
                return Ok(new List<AnuncioDetalleDTO>());

            return Ok(resultado.Valor);
        }

        // ==================== Crear anuncio ====================
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CrearAnuncio([FromForm] CrearAnuncioDTO dto)
        {
            if (!User.TieneRol("1", "2")) // Solo superusuario y administrador
                return Unauthorized(new { error = "No tienes permisos para crear anuncios." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Imagenes == null || dto.Imagenes.Length == 0)
                return BadRequest(new { error = "Debe proporcionar al menos una imagen para el anuncio." });

            try
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "anuncios");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                var urlsImagenes = new List<string>();

                foreach (var imagen in dto.Imagenes)
                {
                    var extension = Path.GetExtension(imagen.FileName).ToLower();
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                        return BadRequest(new { error = "Solo se permiten imágenes JPG, JPEG o PNG." });

                    if (imagen.Length > 5 * 1024 * 1024)
                        return BadRequest(new { error = "El tamaño máximo permitido por imagen es 5 MB." });

                    var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                    var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using var stream = new FileStream(rutaCompleta, FileMode.Create);
                    await imagen.CopyToAsync(stream);

                    urlsImagenes.Add($"/imagenes/anuncios/{nombreArchivo}");
                }

                var anuncio = new Anuncio
                {
                    Titulo = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    ImagenUrl = string.Join(";", urlsImagenes),
                    EsPasantia = dto.EsPasantia,
                    FechaPublicacion = DateTime.Now
                };

                var creado = await _anuncioServicio.CrearAsync(anuncio);
                if (!creado.esExitoso)
                    return StatusCode(500, new { error = creado.MensajeError });

                return Ok(new { mensaje = "Anuncio creado correctamente.", anuncio });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al crear el anuncio.", detalle = ex.Message });
            }
        }

        // ==================== Actualizar anuncio ====================
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> ActualizarAnuncio(int id, [FromForm] ActualizarAnuncioDTO dto)
        {
            if (!User.TieneRol("1", "2")) // Solo superusuario y administrador
                return Unauthorized(new { error = "No tienes permisos para actualizar anuncios." });

            if (dto == null)
                return BadRequest(new { error = "Los datos del anuncio no pueden estar vacíos." });

            var resultadoExistente = await _anuncioServicio.ObtenerPorIdAsync(id);
            if (!resultadoExistente.esExitoso || resultadoExistente.Valor == null)
                return NotFound(new { error = $"No se encontró el anuncio con ID {id}" });

            var anuncioExistente = resultadoExistente.Valor;
            string nuevaUrlImagen = anuncioExistente.ImagenUrl;

            if (dto.Imagen != null && dto.Imagen.Length > 0)
            {
                var extension = Path.GetExtension(dto.Imagen.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    return BadRequest(new { error = "Solo se permiten imágenes JPG, JPEG o PNG." });

                if (dto.Imagen.Length > 5 * 1024 * 1024)
                    return BadRequest(new { error = "El tamaño máximo permitido para la imagen es 5 MB." });

                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "anuncios");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using var stream = new FileStream(rutaCompleta, FileMode.Create);
                await dto.Imagen.CopyToAsync(stream);

                nuevaUrlImagen = $"/imagenes/anuncios/{nombreArchivo}";
            }

            dto.ImagenUrl = nuevaUrlImagen;
            var actualizado = await _anuncioServicio.ActualizarAsync(id, dto);

            if (!actualizado.esExitoso)
                return StatusCode(500, new { error = actualizado.MensajeError });

            return Ok(new { mensaje = "Anuncio actualizado correctamente.", anuncio = dto });
        }

        // ==================== Eliminar anuncio ====================
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> EliminarAnuncio(int id)
        {
            if (!User.TieneRol("1", "2")) // Solo superusuario y administrador
                return Unauthorized(new { error = "No tienes permisos para eliminar anuncios." });

            var eliminado = await _anuncioServicio.EliminarAsync(id);
            if (!eliminado.esExitoso)
                return NotFound(new { error = eliminado.MensajeError });

            return Ok(new { mensaje = "Anuncio eliminado correctamente." });
        }

        // ==================== Ver currículums ====================
        [HttpGet("{id}/curriculums")]
        [Authorize]
        public async Task<IActionResult> VerCurriculums(int id)
        {
            if (!User.TieneRol("1", "2")) // Solo superusuario y administrador
                return Unauthorized(new { error = "No tienes permisos para ver currículums." });

            var resultado = await _anuncioServicio.ObtenerPorIdAsync(id);
            if (!resultado.esExitoso || resultado.Valor == null)
                return NotFound(new { error = "Anuncio no encontrado." });

            var anuncio = resultado.Valor;

            if (!anuncio.EsPasantia)
                return Ok(new { mensaje = "Este anuncio no es de pasantía.", curriculums = new List<string>() });

            var curriculums = await _anuncioServicio.ObtenerCurriculumsAsync(id);
            if (!curriculums.esExitoso)
                return StatusCode(500, new { error = curriculums.MensajeError });

            return Ok(curriculums.Valor ?? new List<string>());
        }
    }
}
