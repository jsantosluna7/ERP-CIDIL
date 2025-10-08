using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Implementaciones.Servicios
{
    public class LikeServicio : ILikeServicio
    {
        private readonly ILikeRepositorio _repo;

        public LikeServicio(ILikeRepositorio repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Obtiene todos los likes y los transforma a LikeDTO.
        /// </summary>
        public async Task<List<LikeDTO>> ObtenerTodosAsync()
        {
            var likes = await _repo.ObtenerTodosAsync();
            return likes.Select(l => new LikeDTO
            {
                AnuncioId = l.AnuncioId,
                Usuario = l.Usuario
            }).ToList();
        }

        /// <summary>
        /// Obtiene un like por su Id.
        /// </summary>
        public async Task<LikeDTO?> ObtenerPorIdAsync(int id)
        {
            var like = await _repo.ObtenerPorIdAsync(id);
            if (like == null) return null;

            return new LikeDTO
            {
                AnuncioId = like.AnuncioId,
                Usuario = like.Usuario
            };
        }

        /// <summary>
        /// Crea un like asegurándose de que no se generen errores crudos.
        /// </summary>
        public async Task<bool> CrearAsync(LikeDTO dto)
        {
            try
            {
                var like = new Like
                {
                    AnuncioId = dto.AnuncioId,
                    Usuario = dto.Usuario
                };

                await _repo.CrearAsync(like);
                await _repo.GuardarAsync();
                return true;
            }
            catch (InvalidOperationException ex)
            {
                // Aquí puedes loguear el error si quieres
                // Por ejemplo: _logger.LogWarning(ex.Message);
                // Devuelve false indicando que no se pudo crear
                return false;
            }
        }

        /// <summary>
        /// Elimina un like por su Id.
        /// </summary>
        public async Task<bool> EliminarAsync(int id)
        {
            var eliminado = await _repo.EliminarAsync(id);
            await _repo.GuardarAsync();
            return eliminado;
        }

        /// <summary>
        /// Cuenta los likes de un anuncio específico.
        /// </summary>
        public async Task<int> ContarPorAnuncioAsync(int anuncioId)
        {
            var likes = await _repo.ObtenerTodosAsync();
            return likes.Count(l => l.AnuncioId == anuncioId);
        }
    }
}
