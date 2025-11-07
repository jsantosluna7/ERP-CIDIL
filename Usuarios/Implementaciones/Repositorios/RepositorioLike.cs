using ERP.Data;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioLike : ILikeRepositorio
    {
        private readonly DbErpContext _context;

        public RepositorioLike(DbErpContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

 
        // Obtiene todos los likes.
      
        public async Task<Resultado<List<Like>>> ObtenerTodosAsync()
        {
            try
            {
                var likes = await _context.Likes.ToListAsync();
                if (likes == null || !likes.Any())
                    return Resultado<List<Like>>.Falla("No hay likes registrados.");

                return Resultado<List<Like>>.Exito(likes);
            }
            catch (Exception ex)
            {
                return Resultado<List<Like>>.Falla($"Error al obtener likes");
            }
        }


        // Obtiene un like por su Id.
    
        public async Task<Resultado<Like>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var like = await _context.Likes.FindAsync(id);
                if (like == null)
                    return Resultado<Like>.Falla($"No se encontró un like con el Id  ");

                return Resultado<Like>.Exito(like);
            }
            catch (Exception ex)
            {
                return Resultado<Like>.Falla($"Error al obtener el like");
            }
        }

     
        // Busca si un usuario (por correo institucional) ya ha dado like a un anuncio.
     
        public async Task<Resultado<Like>> ObtenerPorAnuncioYUsuarioAsync(int anuncioId, string correoUsuario)
        {
            try
            {
                var like = await _context.Likes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l =>
                        l.AnuncioId == anuncioId &&
                        l.Usuario != null &&
                        l.Usuario.CorreoInstitucional.ToLower() == correoUsuario.ToLower());

                if (like == null)
                    return Resultado<Like>.Falla("No se encontró like de este usuario en el anuncio.");

                return Resultado<Like>.Exito(like);
            }
            catch (Exception ex)
            {
                return Resultado<Like>.Falla($"Error al buscar like");
            }
        }

        
        // Crea un nuevo like.
      
        public async Task<Resultado<bool>> CrearAsync(Like like)
        {
            try
            {
                await _context.Likes.AddAsync(like);
                var guardado = await _context.SaveChangesAsync() > 0;
                if (!guardado) return Resultado<bool>.Falla("No se pudo crear el like.");

                return Resultado<bool>.Exito(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Falla($"Error al crear like ");
            }
        }

       
        // Elimina un like por Id.
       
        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var like = await _context.Likes.FindAsync(id);
                if (like == null)
                    return Resultado<bool>.Falla("No se encontró el like a eliminar.");

                _context.Likes.Remove(like);
                var guardado = await _context.SaveChangesAsync() > 0;
                if (!guardado) return Resultado<bool>.Falla("No se pudo eliminar el like.");

                return Resultado<bool>.Exito(true);
            }
            catch (Exception ex)
            {
                return Resultado<bool>.Falla($"Error al eliminar like");
            }
        }

        
        // Cuenta los likes de un anuncio.
        
        public async Task<Resultado<int>> ContarPorAnuncioAsync(int anuncioId)
        {
            try
            {
                var count = await _context.Likes.CountAsync(l => l.AnuncioId == anuncioId);
                return Resultado<int>.Exito(count);
            }
            catch (Exception ex)
            {
                return Resultado<int>.Falla($"Error al contar likes");
            }
        }
    }
}
