using ERP.Data;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    public class LikeRepositorio : ILikeRepositorio
    {
        private readonly DbErpContext _context;

        public LikeRepositorio(DbErpContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los likes incluyendo el anuncio relacionado.
        /// </summary>
        public async Task<List<Like>> ObtenerTodosAsync()
        {
            return await _context.Likes
                .Include(l => l.Anuncio)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un like específico por su Id.
        /// </summary>
        public async Task<Like?> ObtenerPorIdAsync(int id)
        {
            return await _context.Likes
                .Include(l => l.Anuncio)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        /// <summary>
        /// Crea un nuevo like en la base de datos.
        /// Devuelve true si se guardó correctamente.
        /// </summary>
        public async Task<bool> CrearAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Elimina un like por su Id.
        /// Devuelve true si se eliminó correctamente.
        /// </summary>
        public async Task<bool> EliminarAsync(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null)
                return false;

            _context.Likes.Remove(like);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Cuenta cuántos likes tiene un anuncio específico.
        /// </summary>
        public async Task<int> ContarPorAnuncioAsync(int anuncioId)
        {
            return await _context.Likes
                .CountAsync(l => l.AnuncioId == anuncioId);
        }

        /// <summary>
        /// Guarda los cambios pendientes en la base de datos.
        /// </summary>
        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
