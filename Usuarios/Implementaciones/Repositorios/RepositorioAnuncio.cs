using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{

    // Implementación concreta del repositorio de anuncios.
    // Gestiona la persistencia de los anuncios y currículums en la base de datos.

    public class RepositorioAnuncio : IAnuncioRepositorio
    {
        private readonly DbErpContext _context;

        public RepositorioAnuncio(DbErpContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        // Obtiene todos los anuncios registrados.

        public async Task<Resultado<List<Anuncio>>> ObtenerTodosAsync()
        {
            try
            {
                var anuncios = await _context.Anuncios.ToListAsync();

                // 🛑 CORRECCIÓN: Carga explícita manual del Usuario para cada anuncio
                foreach (var anuncio in anuncios)
                {
                    await _context.Entry(anuncio).Reference(a => a.Usuario).LoadAsync();
                }

                return Resultado<List<Anuncio>>.Exito(anuncios);
            }
            catch
            {
                return Resultado<List<Anuncio>>.Falla("Error al obtener anuncios.");
            }
        }


        // Obtiene un anuncio por su ID.

        public async Task<Resultado<Anuncio>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var anuncio = await _context.Anuncios.FirstOrDefaultAsync(a => a.Id == id);

                if (anuncio == null)
                    return Resultado<Anuncio>.Falla("El anuncio no existe.");

                // 🛑 CORRECCIÓN: Carga explícita manual del Usuario para este anuncio
                await _context.Entry(anuncio).Reference(a => a.Usuario).LoadAsync();

                return Resultado<Anuncio>.Exito(anuncio);
            }
            catch
            {
                return Resultado<Anuncio>.Falla("Error al obtener el anuncio.");
            }
        }


        // Crea un nuevo anuncio.

        public async Task<Resultado<bool>> CrearAsync(Anuncio anuncio)
        {
            try
            {
                await _context.Anuncios.AddAsync(anuncio);
                await GuardarAsync();
                return Resultado<bool>.Exito(true);
            }
            catch
            {
                return Resultado<bool>.Falla("Error al crear el anuncio.");
            }
        }


        // Actualiza un anuncio existente.

        public async Task<Resultado<bool>> ActualizarAsync(Anuncio anuncio)
        {
            try
            {
                _context.Anuncios.Update(anuncio);
                await GuardarAsync();
                return Resultado<bool>.Exito(true);
            }
            catch
            {
                return Resultado<bool>.Falla("Error al actualizar el anuncio.");
            }
        }


        // Elimina un anuncio por su ID.

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
            catch
            {
                return Resultado<bool>.Falla("Error al eliminar el anuncio.");
            }
        }


        // Guarda los cambios en la base de datos.

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }


        // Alterna (agrega o quita) un "like" de un usuario sobre un anuncio.

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
            catch
            {
                return Resultado<bool>.Falla("Error al alternar like.");
            }
        }

        /// <summary>
        /// Obtiene los comentarios y likes de un anuncio.
        /// </summary>
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
            catch
            {
                return Resultado<(List<Comentario>, List<Like>)>.Falla("Error al obtener comentarios y likes.");
            }
        }


        // Obtiene los currículums asociados a un anuncio.

        public async Task<Resultado<List<Curriculum>>> ObtenerCurriculumsAsync(int anuncioId)
        {
            try
            {
                var curriculums = await _context.Curriculums
                  .Where(c => c.AnuncioId == anuncioId)
                  .ToListAsync();

                return Resultado<List<Curriculum>>.Exito(curriculums);
            }
            catch
            {
                return Resultado<List<Curriculum>>.Falla("Error al obtener currículums.");
            }
        }


        // Agrega un nuevo currículum.

        public async Task<Resultado<bool>> AgregarCurriculumAsync(Curriculum curriculum)
        {
            try
            {
                await _context.Curriculums.AddAsync(curriculum);
                await GuardarAsync();
                return Resultado<bool>.Exito(true);
            }
            catch
            {
                return Resultado<bool>.Falla("Error al agregar currículum.");
            }
        }
    }
}