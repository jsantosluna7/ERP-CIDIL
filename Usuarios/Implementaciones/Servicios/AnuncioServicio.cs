using ERP.Data.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Implementaciones.Servicios
{
    /// <summary>
    /// Servicio encargado de la lógica de negocio de los anuncios.
    /// Gestiona la creación, actualización, eliminación y obtención de anuncios.
    /// </summary>
    public class AnuncioServicio : IAnuncioServicio
    {
        private readonly IAnuncioRepositorio _repo;

        /// <summary>
        /// Constructor del servicio de anuncios.
        /// </summary>
        /// <param name="repo">Repositorio de anuncios.</param>
        public AnuncioServicio(IAnuncioRepositorio repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Obtiene todos los anuncios con detalle, incluyendo comentarios y cantidad de likes.
        /// </summary>
        /// <returns>Lista de objetos <see cref="AnuncioDetalleDTO"/>.</returns>
        public async Task<List<AnuncioDetalleDTO>> ObtenerTodosAsync()
        {
            var anuncios = await _repo.ObtenerTodosAsync();

            if (anuncios == null || !anuncios.Any())
                return new List<AnuncioDetalleDTO>();

            return anuncios.Select(a => new AnuncioDetalleDTO
            {
                Id = a.Id,
                Titulo = a.Titulo ?? string.Empty,
                Descripcion = a.Descripcion ?? string.Empty,
                ImagenUrl = a.ImagenUrl ?? string.Empty,
                FechaPublicacion = a.FechaPublicacion,
                CantidadLikes = a.Likes?.Count ?? 0,
                Comentarios = (a.Comentarios ?? Enumerable.Empty<Comentario>())
                    .Select(c => new ComentarioDTO
                    {
                        AnuncioId = a.Id,
                        Usuario = c.Usuario,
                        Texto = c.Texto
                    }).ToList()
            }).ToList();
        }

        /// <summary>
        /// Crea un nuevo anuncio y guarda la imagen en /wwwroot/imagenes/anuncios/.
        /// </summary>
        /// <param name="dto">Datos del anuncio con la imagen.</param>
        public async Task CrearAsync(CrearAnuncioDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Imagen == null || dto.Imagen.Length == 0)
                throw new Exception("Debe proporcionar una imagen para el anuncio.");

            // Validar extensión de imagen
            var extension = Path.GetExtension(dto.Imagen.FileName).ToLowerInvariant();
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png" };

            if (!extensionesPermitidas.Contains(extension))
                throw new Exception("Solo se permiten imágenes en formato JPG, JPEG o PNG.");

            // Validar tamaño máximo (3 MB)
            if (dto.Imagen.Length > 3 * 1024 * 1024)
                throw new Exception("La imagen es demasiado grande. El tamaño máximo permitido es de 3 MB.");

            // Crear carpeta si no existe
            var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "anuncios");
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            // Guardar imagen con nombre único
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            await using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await dto.Imagen.CopyToAsync(stream);
            }

            // URL accesible públicamente
            var urlImagen = $"/imagenes/anuncios/{nombreArchivo}";

            // Crear entidad de anuncio
            var anuncio = new Anuncio
            {
                Titulo = dto.Titulo?.Trim() ?? string.Empty,
                Descripcion = dto.Descripcion?.Trim() ?? string.Empty,
                ImagenUrl = urlImagen,
                FechaPublicacion = DateTime.UtcNow
            };

            await _repo.CrearAsync(anuncio);
            await _repo.GuardarAsync();
        }

        /// <summary>
        /// Actualiza un anuncio existente. Puede actualizar también la URL de la imagen.
        /// </summary>
        /// <param name="id">ID del anuncio.</param>
        /// <param name="dto">Datos a actualizar.</param>
        /// <returns>True si se actualizó correctamente, false en caso contrario.</returns>
        public async Task<bool> ActualizarAsync(int id, ActualizarAnuncioDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var anuncio = await _repo.ObtenerPorIdAsync(id);
            if (anuncio == null)
                return false;

            anuncio.Titulo = dto.Titulo?.Trim() ?? anuncio.Titulo;
            anuncio.Descripcion = dto.Descripcion?.Trim() ?? anuncio.Descripcion;

            if (!string.IsNullOrWhiteSpace(dto.ImagenUrl))
                anuncio.ImagenUrl = dto.ImagenUrl.Trim();

            _repo.Actualizar(anuncio);
            await _repo.GuardarAsync();

            return true;
        }

        /// <summary>
        /// Elimina un anuncio existente por su ID y borra la imagen asociada del disco.
        /// </summary>
        /// <param name="id">ID del anuncio a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        public async Task<bool> EliminarAsync(int id)
        {
            var anuncio = await _repo.ObtenerPorIdAsync(id);
            if (anuncio == null)
                return false;

            // Eliminar imagen del disco si existe
            if (!string.IsNullOrWhiteSpace(anuncio.ImagenUrl))
            {
                var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", anuncio.ImagenUrl.TrimStart('/'));
                if (File.Exists(rutaImagen))
                {
                    try
                    {
                        File.Delete(rutaImagen);
                    }
                    catch
                    {
                        // No detener el flujo si ocurre un error al borrar la imagen
                    }
                }
            }

            _repo.Eliminar(anuncio);
            await _repo.GuardarAsync();
            return true;
        }
    }
}
