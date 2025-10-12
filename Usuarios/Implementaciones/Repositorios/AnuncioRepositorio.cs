using ERP.Data.Modelos; // DbErpContext y modelos están aquí
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    /// <summary>
    /// Implementación del repositorio para la gestión de anuncios y sus "likes".
    /// </summary>
    public class AnuncioRepositorio : IAnuncioRepositorio
    {
        private readonly DbErpContext _context;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public AnuncioRepositorio(DbErpContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene todos los anuncios incluyendo comentarios y likes.
        /// </summary>
        public async Task<List<Anuncio>> ObtenerTodosAsync()
        {
            return await _context.Anuncios
                .Include(a => a.Comentarios)
                .Include(a => a.Likes)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un anuncio por su ID, incluyendo comentarios y likes.
        /// </summary>
        public async Task<Anuncio?> ObtenerPorIdAsync(int id)
        {
            return await _context.Anuncios
                .Include(a => a.Comentarios)
                .Include(a => a.Likes)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <summary>
        /// Crea un nuevo anuncio.
        /// </summary>
        public async Task CrearAsync(Anuncio anuncio)
        {
            if (anuncio == null)
                throw new ArgumentNullException(nameof(anuncio));

            await _context.Anuncios.AddAsync(anuncio);
        }

        /// <summary>
        /// Actualiza un anuncio existente.
        /// </summary>
        public void Actualizar(Anuncio anuncio)
        {
            if (anuncio == null)
                throw new ArgumentNullException(nameof(anuncio));

            _context.Anuncios.Update(anuncio);
        }

        /// <summary>
        /// Elimina un anuncio existente.
        /// </summary>
        public void Eliminar(Anuncio anuncio)
        {
            if (anuncio == null)
                throw new ArgumentNullException(nameof(anuncio));

            _context.Anuncios.Remove(anuncio);
        }

        /// <summary>
        /// Guarda los cambios realizados en la base de datos.
        /// </summary>
        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Alterna (añade o quita) un "like" según si el usuario ya lo dio o no.
        /// </summary>
        /// <param name="anuncioId">ID del anuncio</param>
        /// <param name="usuario">Nombre del usuario (string)</param>
        /// <returns>True si el like fue añadido, False si fue quitado</returns>
        public async Task<bool> ToggleLikeAsync(int anuncioId, string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El usuario no puede ser nulo o vacío.", nameof(usuario));

            // Verifica si el usuario ya dio like
            var likeExistente = await _context.Likes
                .FirstOrDefaultAsync(l => l.AnuncioId == anuncioId && l.Usuario == usuario);

            if (likeExistente != null)
            {
                // Si existe, se elimina el like
                _context.Likes.Remove(likeExistente);
                await _context.SaveChangesAsync();
                return false; // Like eliminado
            }

            // Si no existe, se crea un nuevo like
            var nuevoLike = new Like
            {
                AnuncioId = anuncioId,
                Usuario = usuario,
                Fecha = DateTime.UtcNow
            };

            await _context.Likes.AddAsync(nuevoLike);
            await _context.SaveChangesAsync();
            return true; // Like agregado
        }
    }
}
