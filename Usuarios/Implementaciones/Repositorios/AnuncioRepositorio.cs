using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{

    /// Implementación concreta del repositorio de anuncios.
    /// Gestiona la persistencia de los anuncios en la base de datos.
   
    public class AnuncioRepositorio : IAnuncioRepositorio
    {
        private readonly DbErpContext _context;

        public AnuncioRepositorio(DbErpContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Obtiene todos los anuncios registrados.
        /// </summary>
        public async Task<Resultado<List<Anuncio>>> ObtenerTodosAsync()
        {
            try
            {
                var anuncios = await _context.Anuncios.ToListAsync();
                return Resultado<List<Anuncio>>.Exito(anuncios);
            }
            catch (Exception ex)
            {
                return Resultado<List<Anuncio>>.Falla("Error al obtener anuncios.");
            }
        }

        /// <summary>
        /// Obtiene un anuncio por su ID.
        /// </summary>
        public async Task<Resultado<Anuncio>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var anuncio = await _context.Anuncios
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (anuncio == null)
                    return Resultado<Anuncio>.Falla("El anuncio no existe.");

                return Resultado<Anuncio>.Exito(anuncio);
            }
            catch (Exception ex)
            {
                return Resultado<Anuncio>.Falla("Error al obtener el anuncio.");
            }
        }

        /// <summary>
        /// Crea un nuevo anuncio.
        /// </summary>
        public async Task<Resultado<bool>> CrearAsync(Anuncio anuncio)
        {
            try
            {
                await _context.Anuncios.AddAsync(anuncio);
                await GuardarAsync();
                return Resultado<bool>.Exito(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Falla("Error al crear el anuncio.");
            }
        }

        /// <summary>
        /// Actualiza un anuncio existente.
        /// </summary>
        public async Task<Resultado<bool>> ActualizarAsync(Anuncio anuncio)
        {
            try
            {
                _context.Anuncios.Update(anuncio);
                await GuardarAsync();
                return Resultado<bool>.Exito(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Falla("Error al actualizar el anuncio.");
            }
        }

       
        /// Elimina un anuncio por su ID.
        
        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var anuncio = await _context.Anuncios.FindAsync(id);
                if (anuncio == null)
                    return Resultado<bool>.Falla("El anuncio no existe.");

                _context.Anuncios.Remove(anuncio);
                await GuardarAsync();
                return Resultado<bool>.Exito(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Falla("Error al eliminar el anuncio.");
            }
        }

      
        /// Guarda los cambios en la base de datos.
    
        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }

      
        /// Alterna (agrega o quita) un "like" de un usuario sobre un anuncio.
     
        public async Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId)
        {
            try
            {
                var like = await _context.Likes
                    .FirstOrDefaultAsync(l => l.AnuncioId == anuncioId && l.UsuarioId == usuarioId);

                if (like != null)
                {
                    _context.Likes.Remove(like);
                    await GuardarAsync();
                    return Resultado<bool>.Exito(false); // Like eliminado
                }

                var nuevoLike = new Like
                {
                    AnuncioId = anuncioId,
                    UsuarioId = usuarioId,
                    Fecha = DateTime.Now
                };

                await _context.Likes.AddAsync(nuevoLike);
                await GuardarAsync();
                return Resultado<bool>.Exito(true); // Like agregado
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Falla("Error al alternar like.");
            }
        }


        /// Obtiene los comentarios y likes de un anuncio.
      
        public async Task<Resultado<(List<Comentario> Comentarios, List<Like> Likes)>> ObtenerComentariosYLikesAsync(int anuncioId)
        {
            try
            {
                var comentarios = await _context.Comentarios
                    .Where(c => c.AnuncioId == anuncioId)
                    .ToListAsync();

                var likes = await _context.Likes
                    .Where(l => l.AnuncioId == anuncioId)
                    .ToListAsync();

                return Resultado<(List<Comentario>, List<Like>)>.Exito((comentarios, likes));
            }
            catch (Exception ex)
            {
                return Resultado<(List<Comentario>, List<Like>)>.Falla("Error al obtener comentarios y likes.");
            }
        }
    }
}
