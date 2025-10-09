using ERP.Data.Modelos;
using Usuarios.Abstraccion.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Implementaciones.Repositorios
{
    /// <summary>
    /// Implementación de la interfaz IAnuncioRepositorio para operaciones CRUD de anuncios.
    /// </summary>
    public class AnuncioRepositorio : IAnuncioRepositorio
    {
        private readonly DbErpContext _context;

        public AnuncioRepositorio(DbErpContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los anuncios, incluyendo comentarios y likes asociados.
        /// </summary>
        /// <returns>Lista de anuncios completa.</returns>
        public async Task<List<Anuncio>> ObtenerTodosAsync()
        {
            return await _context.Anuncios
                .Include(a => a.Comentarios)
                .Include(a => a.Likes)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un anuncio específico por su ID, incluyendo comentarios y likes.
        /// </summary>
        /// <param name="id">Identificador del anuncio.</param>
        /// <returns>Anuncio encontrado o null si no existe.</returns>
        public async Task<Anuncio?> ObtenerPorIdAsync(int id)
        {
            return await _context.Anuncios
                .Include(a => a.Comentarios)
                .Include(a => a.Likes)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <summary>
        /// Crea un nuevo anuncio en la base de datos.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio a crear.</param>
        public async Task CrearAsync(Anuncio anuncio)
        {
            await _context.Anuncios.AddAsync(anuncio);
        }

        /// <summary>
        /// Actualiza un anuncio existente.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio con cambios.</param>
        public void Actualizar(Anuncio anuncio)
        {
            _context.Anuncios.Update(anuncio);
        }

        /// <summary>
        /// Elimina un anuncio existente.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio a eliminar.</param>
        public void Eliminar(Anuncio anuncio)
        {
            _context.Anuncios.Remove(anuncio);
        }

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos.
        /// </summary>
        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
