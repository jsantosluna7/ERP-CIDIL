using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Modelos;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Implementaciones.Servicios
{
    public class AnuncioServicio : IAnuncioServicio
    {
        private readonly IAnuncioRepositorio _repo;

        public AnuncioServicio(IAnuncioRepositorio repo)
        {
            _repo = repo;
        }

        // Obtener todos los anuncios con detalle
        public async Task<List<AnuncioDetalleDTO>> ObtenerTodosAsync()
        {
            var anuncios = await _repo.ObtenerTodosAsync();

            return anuncios.Select(a => new AnuncioDetalleDTO
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Descripcion = a.Descripcion,
                ImagenUrl = a.ImagenUrl,
                FechaPublicacion = a.FechaPublicacion,
                CantidadLikes = a.Likes?.Count ?? 0, // Cuenta los Likes
                Comentarios = (a.Comentarios ?? Enumerable.Empty<Comentario>())
                    .Select(c => new ComentarioDTO
                    {
                        AnuncioId = a.Id,
                        Usuario = c.Usuario,
                        Texto = c.Texto
                    }).ToList()
            }).ToList();
        }

        // Crear un anuncio
        public async Task CrearAsync(CrearAnuncioDTO dto)
        {
            var anuncio = new Anuncio
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                ImagenUrl = dto.ImagenUrl
            };

            await _repo.CrearAsync(anuncio);
            await _repo.GuardarAsync();
        }

        // Actualizar un anuncio
        public async Task<bool> ActualizarAsync(int id, ActualizarAnuncioDTO dto)
        {
            var anuncio = await _repo.ObtenerPorIdAsync(id);
            if (anuncio == null) return false;

            anuncio.Titulo = dto.Titulo;
            anuncio.Descripcion = dto.Descripcion;
            anuncio.ImagenUrl = dto.ImagenUrl;

            _repo.Actualizar(anuncio);
            await _repo.GuardarAsync();
            return true;
        }

        // Eliminar un anuncio
        public async Task<bool> EliminarAsync(int id)
        {
            var anuncio = await _repo.ObtenerPorIdAsync(id);
            if (anuncio == null) return false;

            _repo.Eliminar(anuncio);
            await _repo.GuardarAsync();
            return true;
        }
    }
}
