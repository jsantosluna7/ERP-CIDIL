using ERP.Data.Modelos;
using Usuarios.Abstraccion.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Implementaciones.Repositorios
{
    public class AnuncioRepositorio : IAnuncioRepositorio
    {
        private readonly DbErpContext _context;

        public AnuncioRepositorio(DbErpContext context)
        {
            _context = context;
        }

        // Obtener todos los anuncios con comentarios y likes
        public async Task<List<Anuncio>> ObtenerTodosAsync()
        {
            return await _context.Anuncios
                .Include(a => a.Comentarios)
                .Include(a => a.Likes) // EF Core ahora mapea Likes correctamente
                .ToListAsync();
        }

        // Obtener un anuncio por Id
        public async Task<Anuncio?> ObtenerPorIdAsync(int id)
        {
            return await _context.Anuncios
                .Include(a => a.Comentarios)
                .Include(a => a.Likes)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Crear un nuevo anuncio
        public async Task CrearAsync(Anuncio anuncio)
        {
            await _context.Anuncios.AddAsync(anuncio);
        }

        // Actualizar un anuncio existente
        public void Actualizar(Anuncio anuncio)
        {
            _context.Anuncios.Update(anuncio);
        }

        // Eliminar un anuncio
        public void Eliminar(Anuncio anuncio)
        {
            _context.Anuncios.Remove(anuncio);
        }

        // Guardar cambios en la base de datos
        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
