using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioComentario : IRepositorioComentario
    {
        private readonly DbErpContext _context;

        public RepositorioComentario(DbErpContext context)
        {
            _context = context;
        }

        
        // Obtiene todos los comentarios.
      
        public async Task<Resultado<List<Comentario>>> ObtenerTodosAsync()
        {
            try
            {
                var comentarios = await _context.Comentarios
                    .OrderByDescending(c => c.Fecha)
                    .ToListAsync();

                return Resultado<List<Comentario>>.Exito(comentarios);
            }
            catch (System.Exception ex)
            {
                return Resultado<List<Comentario>>.Falla(ex.Message);
            }
        }

   
        // Obtiene un comentario por su ID.

        public async Task<Resultado<Comentario>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var comentario = await _context.Comentarios
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (comentario == null)
                    return Resultado<Comentario>.Falla("Comentario no encontrado.");

                return Resultado<Comentario>.Exito(comentario);
            }
            catch (System.Exception ex)
            {
                return Resultado<Comentario>.Falla(ex.Message);
            }
        }

       
        // Obtiene los comentarios asociados a un anuncio específico.
        
        public async Task<Resultado<List<Comentario>>> ObtenerPorAnuncioAsync(int anuncioId)
        {
            try
            {
                var comentarios = await _context.Comentarios
                    .Where(c => c.AnuncioId == anuncioId)
                    .OrderByDescending(c => c.Fecha)
                    .ToListAsync();

                return Resultado<List<Comentario>>.Exito(comentarios);
            }
            catch (System.Exception ex)
            {
                return Resultado<List<Comentario>>.Falla(ex.Message);
            }
        }

        
        // Crea un nuevo comentario en la base de datos.
        
        public async Task<Resultado<Comentario>> CrearAsync(Comentario comentario)
        {
            try
            {
                await _context.Comentarios.AddAsync(comentario);
                await _context.SaveChangesAsync();
                return Resultado<Comentario>.Exito(comentario);
            }
            catch (System.Exception ex)
            {
                return Resultado<Comentario>.Falla(ex.Message);
            }
        }

        
        // Actualiza la información de un comentario existente.
       
        public async Task<Resultado<bool>> ActualizarAsync(Comentario comentario)
        {
            try
            {
                _context.Comentarios.Update(comentario);
                await _context.SaveChangesAsync();
                return Resultado<bool>.Exito(true);
            }
            catch (System.Exception ex)
            {
                return Resultado<bool>.Falla(ex.Message);
            }
        }

       
        // Elimina un comentario por su ID.
      
        public async Task<Resultado<bool>> EliminarPorIdAsync(int id)
        {
            try
            {
                var comentario = await _context.Comentarios.FindAsync(id);
                if (comentario == null)
                    return Resultado<bool>.Falla("Comentario no encontrado.");

                _context.Comentarios.Remove(comentario);
                await _context.SaveChangesAsync();
                return Resultado<bool>.Exito(true);
            }
            catch (System.Exception ex)
            {
                return Resultado<bool>.Falla(ex.Message);
            }
        }

        
        // Devuelve un IQueryable de comentarios para consultas personalizadas.
       
        public IQueryable<Comentario> ObtenerQueryable()
        {
            return _context.Comentarios.AsQueryable();
        }
    }
}
