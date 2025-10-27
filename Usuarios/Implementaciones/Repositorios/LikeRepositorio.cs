using ERP.Data;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        /// Obtiene todos los likes incluyendo relaciones.
        /// </summary>
        public async Task<List<Like>> ObtenerTodosAsync()
        {
            return await _context.Likes
                .Include(l => l.Anuncio)
                .Include(l => l.Usuario) // Usuario institucional
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un like por su Id.
        /// </summary>
        public async Task<Like?> ObtenerPorIdAsync(int id)
        {
            return await _context.Likes
                .Include(l => l.Anuncio)
                .Include(l => l.Usuario)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        /// <summary>
        /// Busca si un usuario (por correo institucional) ya ha dado like a un anuncio.
        /// </summary>
        public async Task<Like?> ObtenerPorAnuncioYUsuarioAsync(int anuncioId, string correoUsuario)
        {
            return await _context.Likes
                .Include(l => l.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(l =>
                    l.AnuncioId == anuncioId &&
                    l.Usuario != null &&
                    l.Usuario.CorreoInstitucional.ToLower() == correoUsuario.ToLower());
        }

        /// <summary>
        /// Crea un nuevo like.
        /// </summary>
        public async Task<bool> CrearAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Elimina un like por Id.
        /// </summary>
        public async Task<bool> EliminarAsync(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null) return false;

            _context.Likes.Remove(like);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Cuenta los likes de un anuncio.
        /// </summary>
        public async Task<int> ContarPorAnuncioAsync(int anuncioId)
        {
            return await _context.Likes.CountAsync(l => l.AnuncioId == anuncioId);
        }
    }
}
