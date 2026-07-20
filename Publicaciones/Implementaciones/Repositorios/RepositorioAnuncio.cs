using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Publicaciones.Abstraccion.Repositorios;
using Publicaciones.DTO.AnuncioDTO;

namespace Publicaciones.Implementaciones.Repositorios
{
    public class RepositorioAnuncio : IRepositorioAnuncio
    {
        private readonly DbErpContext _context;

        public RepositorioAnuncio(DbErpContext context)
        {
            _context = context;
        }

        // Obtener todos los anuncios ordenados por fecha de publicación descendente
        public async Task<List<Anuncio>?> GetAnuncios()
        {
            return await _context.Anuncios
                .OrderByDescending(a => a.FechaPublicacion)
                .ToListAsync();
        }

        // Obtener solo los anuncios marcados para el carrusel destacado
        public async Task<List<Anuncio>?> GetCarrusel()
        {
            return await _context.Anuncios
                .Where(a => a.EsCarrusel)
                .OrderByDescending(a => a.FechaPublicacion)
                .Take(5)
                .ToListAsync();
        }

        // Obtener solo las pasantías abiertas
        public async Task<List<Anuncio>?> GetPasantias()
        {
            return await _context.Anuncios
                .Where(a => a.EsPasantia == true)
                .OrderByDescending(a => a.FechaPublicacion)
                .ToListAsync();
        }

        // Obtener un anuncio por ID con sus relaciones
        public async Task<Anuncio?> GetById(int id)
        {
            return await _context.Anuncios
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        // Crear un nuevo anuncio
        public async Task<Anuncio?> Crear(CrearAnuncioDTO dto, int usuarioId)
        {
            var anuncio = new Anuncio
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                ImagenUrl = dto.ImagenUrl,
                EsPasantia = dto.EsPasantia,
                EsCarrusel = dto.EsCarrusel,
                UsuarioId = usuarioId,
                FechaCreacion = DateTime.UtcNow,
                FechaPublicacion = DateTime.UtcNow
            };

            _context.Anuncios.Add(anuncio);
            await _context.SaveChangesAsync();
            return anuncio;
        }

        // Actualizar un anuncio existente
        public async Task<Anuncio?> Actualizar(int id, ActualizarAnuncioDTO dto)
        {
            var anuncio = await GetById(id);
            if (anuncio == null) return null;

            anuncio.Titulo = dto.Titulo;
            anuncio.Descripcion = dto.Descripcion;
            anuncio.ImagenUrl = dto.ImagenUrl;
            anuncio.EsPasantia = dto.EsPasantia;
            anuncio.EsCarrusel = dto.EsCarrusel;

            _context.Update(anuncio);
            await _context.SaveChangesAsync();
            return await GetById(id);
        }

        // Eliminar un anuncio por ID
        public async Task<bool?> Eliminar(int id)
        {
            var anuncio = await GetById(id);
            if (anuncio == null) return null;

            _context.Anuncios.Remove(anuncio);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
