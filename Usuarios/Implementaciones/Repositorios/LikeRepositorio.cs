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

        public async Task<List<Like>> ObtenerTodosAsync()
        {
            return await _context.Likes.Include(l => l.Anuncio).ToListAsync();
        }

        public async Task<Like?> ObtenerPorIdAsync(int id)
        {
            return await _context.Likes.Include(l => l.Anuncio)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Like?> ObtenerPorAnuncioYUsuarioAsync(int anuncioId, string usuario)
        {
            return await _context.Likes
                .AsNoTracking()
                .FirstOrDefaultAsync(l =>
                    l.AnuncioId == anuncioId &&
                    l.Usuario.ToLower() == usuario.ToLower());
        }

        public async Task<bool> CrearAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var like = await _context.Likes.FindAsync(id);
            if (like == null) return false;

            _context.Likes.Remove(like);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> ContarPorAnuncioAsync(int anuncioId)
        {
            return await _context.Likes.CountAsync(l => l.AnuncioId == anuncioId);
        }
    }
}
