using ERP.Data.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Implementaciones.Servicios
{
    /// <summary>
    /// Servicio para manejar "Likes" en anuncios.
    /// Solo usuarios institucionales (representados por Usuario) pueden dar Like.
    /// </summary>
    public class LikeServicio : ILikeServicio
    {
        private readonly ILikeRepositorio _repo;
        private readonly IUsuarioRepositorio _usuarioRepo;

        public LikeServicio(ILikeRepositorio repo, IUsuarioRepositorio usuarioRepo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
        }

        public async Task<List<LikeDTO>> ObtenerTodosAsync()
        {
            var likes = await _repo.ObtenerTodosAsync();
            return likes.Select(l => new LikeDTO
            {
                AnuncioId = l.AnuncioId,
                Usuario = l.Usuario?.CorreoInstitucional ?? "Desconocido"
            }).ToList();
        }

        public async Task<LikeDTO?> ObtenerPorIdAsync(int id)
        {
            var like = await _repo.ObtenerPorIdAsync(id);
            if (like == null) return null;

            return new LikeDTO
            {
                AnuncioId = like.AnuncioId,
                Usuario = like.Usuario?.CorreoInstitucional ?? "Desconocido"
            };
        }

        public async Task<(bool estadoActual, int totalLikes)> CrearAsync(LikeDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Usuario))
                throw new ArgumentException("Usuario obligatorio", nameof(dto.Usuario));

            // Buscar usuario por correo institucional
            var usuario = await _usuarioRepo.ObtenerPorCorreoAsync(dto.Usuario);
            if (usuario == null)
                throw new InvalidOperationException($"No existe un usuario con el correo '{dto.Usuario}'.");

            // Verificar si ya dio like usando correo (string)
            var existente = await _repo.ObtenerPorAnuncioYUsuarioAsync(dto.AnuncioId, usuario.CorreoInstitucional);

            if (existente != null)
            {
                // Ya existe → eliminar
                await _repo.EliminarAsync(existente.Id);
            }
            else
            {
                // No existe → crear
                var nuevoLike = new Like
                {
                    AnuncioId = dto.AnuncioId,
                    UsuarioId = usuario.Id,
                    Fecha = DateTime.UtcNow
                };
                await _repo.CrearAsync(nuevoLike);
            }

            int total = await _repo.ContarPorAnuncioAsync(dto.AnuncioId);
            bool estadoActual = existente == null;
            return (estadoActual, total);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repo.EliminarAsync(id);
        }

        public async Task<int> ContarPorAnuncioAsync(int anuncioId)
        {
            return await _repo.ContarPorAnuncioAsync(anuncioId);
        }

        public async Task<bool> ExisteLikeAsync(int anuncioId, string usuarioCorreo)
        {
            var usuario = await _usuarioRepo.ObtenerPorCorreoAsync(usuarioCorreo);
            if (usuario == null) return false;

            // Usar correo en lugar de Id
            var existente = await _repo.ObtenerPorAnuncioYUsuarioAsync(anuncioId, usuario.CorreoInstitucional);
            return existente != null;
        }
    }
}
