using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    public class ComentarioRepositorio : IComentarioRepositorio
    {
        private readonly DbErpContext _context;

        public ComentarioRepositorio(DbErpContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los comentarios, incluyendo el anuncio y el usuario relacionados.
        /// </summary>
        public async Task<List<Comentario>> ObtenerTodosAsync()
        {
            return await _context.Comentarios
                .Include(c => c.Anuncio)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un comentario por su ID.
        /// </summary>
        public async Task<Comentario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Comentarios
                .Include(c => c.Anuncio)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Obtiene los comentarios asociados a un anuncio específico.
        /// </summary>
        public async Task<List<Comentario>> ObtenerPorAnuncioAsync(int anuncioId)
        {
            return await _context.Comentarios
                .Where(c => c.AnuncioId == anuncioId)
                .Include(c => c.Anuncio)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        /// <summary>
        /// Crea un nuevo comentario en la base de datos.
        /// </summary>
        public async Task CrearAsync(Comentario comentario)
        {
            await _context.Comentarios.AddAsync(comentario);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza la información de un comentario existente.
        /// </summary>
        public async Task ActualizarAsync(Comentario comentario)
        {
            _context.Comentarios.Update(comentario);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Elimina un comentario por su ID.
        /// </summary>
        public async Task<bool> EliminarPorIdAsync(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
                return false;

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Guarda los cambios pendientes en la base de datos.
        /// </summary>
        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Devuelve un IQueryable de comentarios para consultas personalizadas.
        /// </summary>
        public IQueryable<Comentario> ObtenerQueryable()
        {
            return _context.Comentarios
                .Include(c => c.Anuncio);
        }
    }
}
