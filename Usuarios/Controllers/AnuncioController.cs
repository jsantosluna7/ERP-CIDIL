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
            var anuncios = await _anuncioServicio.ObtenerTodosAsync(esPasantia);

            if (anuncios == null || !anuncios.Any())
                return Ok(Array.Empty<object>());

            return Ok(anuncios);
        }

        // ==================== Crear anuncio (Solo ADMINISTRADOR/SUPERUSUARIO) ====================
        [HttpPost]
        public async Task<IActionResult> CrearAnuncio([FromForm] CrearAnuncioDTO dto)
        {
            if (!User.TieneRol("ADMINISTRADOR", "SUPERUSUARIO"))
                return Unauthorized("No tienes permisos para crear anuncios.");

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

                    var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                    var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
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

                await _anuncioServicio.CrearAsync(anuncio);
                return Ok(new { mensaje = "Anuncio creado correctamente.", anuncio });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocurrió un error al crear el anuncio.", detalle = ex.Message });
            }
        }

        // ==================== Actualizar anuncio (Solo ADMINISTRADOR/SUPERUSUARIO) ====================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAnuncio(int id, [FromForm] ActualizarAnuncioDTO dto)
        {
            if (!User.TieneRol("ADMINISTRADOR", "SUPERUSUARIO"))
                return Unauthorized("No tienes permisos para actualizar anuncios.");

            if (dto == null)
                return BadRequest(new { error = "Los datos del anuncio no pueden estar vacíos." });

            var anuncioExistente = await _anuncioServicio.ObtenerPorIdAsync(id);
            if (anuncioExistente == null)
                return NotFound(new { error = $"No se encontró el anuncio con ID {id}." });

            string nuevaUrlImagen = anuncioExistente.ImagenUrl;

            if (dto.Imagen != null && dto.Imagen.Length > 0)
            {
                var extension = Path.GetExtension(dto.Imagen.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                    return BadRequest(new { error = "Solo se permiten imágenes JPG, JPEG o PNG." });

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

        // ==================== Eliminar anuncio (Solo ADMINISTRADOR/SUPERUSUARIO) ====================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAnuncio(int id)
        {
            if (!User.TieneRol("ADMINISTRADOR", "SUPERUSUARIO"))
                return Unauthorized("No tienes permisos para eliminar anuncios.");

            var eliminado = await _anuncioServicio.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { error = $"No se encontró el anuncio con ID {id}." });

            return Ok(new { mensaje = "Anuncio eliminado correctamente." });
        }

        // ==================== Ver currículums asociados al anuncio (Solo ADMINISTRADOR/SUPERUSUARIO pueden ver todos) ====================
        [HttpGet("{id}/curriculums")]
        public async Task<IActionResult> VerCurriculums(int id)
        {
            var anuncio = await _anuncioServicio.ObtenerPorIdAsync(id);
            if (anuncio == null)
                return NotFound(new { error = "Anuncio no encontrado." });

            if (!User.TieneRol("ADMINISTRADOR", "SUPERUSUARIO") && !User.TieneRol("ESTUDIANTE", "PROFESOR"))
                return Unauthorized("No tienes permisos para ver currículums.");

            if (!anuncio.EsPasantia)
                return Ok(new { mensaje = "Este anuncio no es de pasantía.", curriculums = Array.Empty<object>() });

            var curriculums = await _anuncioServicio.ObtenerCurriculumsAsync(id);
            return Ok(curriculums);
        }
    }
}
