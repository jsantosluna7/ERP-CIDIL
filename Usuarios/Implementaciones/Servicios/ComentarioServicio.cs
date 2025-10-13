using ERP.Data.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Implementaciones.Servicios
{
    public class ComentarioServicio : IComentarioServicio
    {
        private readonly IComentarioRepositorio _repo;
        private readonly IAnuncioRepositorio _anuncioRepo;
        private readonly IUsuarioPublicoRepositorio _usuarioRepo; // <-- Para validar usuario registrado

        public ComentarioServicio(
            IComentarioRepositorio repo,
            IAnuncioRepositorio anuncioRepo,
            IUsuarioPublicoRepositorio usuarioRepo)
        {
            _repo = repo;
            _anuncioRepo = anuncioRepo;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<List<ComentarioDetalleDTO>> ObtenerTodosAsync()
        {
            var comentarios = await _repo.ObtenerTodosAsync();

            return comentarios.Select(c => new ComentarioDetalleDTO
            {
                Id = c.Id,
                AnuncioId = c.AnuncioId,
                UsuarioId = c.UsuarioId,
                NombreUsuario = c.Usuario?.Nombre,
                Texto = c.Texto,
                Fecha = c.Fecha,
                TituloAnuncio = c.Anuncio?.Titulo
            }).ToList();
        }

        public async Task<ComentarioDetalleDTO?> ObtenerPorIdAsync(int id)
        {
            var comentario = await _repo.ObtenerPorIdAsync(id);
            if (comentario == null) return null;

            return new ComentarioDetalleDTO
            {
                Id = comentario.Id,
                AnuncioId = comentario.AnuncioId,
                UsuarioId = comentario.UsuarioId,
                NombreUsuario = comentario.Usuario?.Nombre,
                Texto = comentario.Texto,
                Fecha = comentario.Fecha,
                TituloAnuncio = comentario.Anuncio?.Titulo
            };
        }

        public async Task<List<ComentarioDetalleDTO>> ObtenerPorAnuncioIdAsync(int anuncioId)
        {
            var comentarios = await _repo.ObtenerPorAnuncioAsync(anuncioId);

            return comentarios.Select(c => new ComentarioDetalleDTO
            {
                Id = c.Id,
                AnuncioId = c.AnuncioId,
                UsuarioId = c.UsuarioId,
                NombreUsuario = c.Usuario?.Nombre,
                Texto = c.Texto,
                Fecha = c.Fecha,
                TituloAnuncio = c.Anuncio?.Titulo
            }).ToList();
        }

        /// <summary>
        /// ✅ Devuelve el comentario recién creado, validando que el usuario esté registrado
        /// </summary>
        public async Task<ComentarioDetalleDTO> CrearAsync(CrearComentarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Texto))
                throw new ArgumentException("El texto del comentario no puede estar vacío.");

            // Validar anuncio
            var anuncio = await _anuncioRepo.ObtenerPorIdAsync(dto.AnuncioId);
            if (anuncio == null)
                throw new KeyNotFoundException($"No existe un anuncio con Id = {dto.AnuncioId}");

            // Validar usuario
            var usuario = await _usuarioRepo.ObtenerPorIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"No existe un usuario registrado con Id = {dto.UsuarioId}");

            var comentario = new Comentario
            {
                AnuncioId = dto.AnuncioId,
                UsuarioId = dto.UsuarioId,
                Usuario = usuario, // navegación
                Texto = dto.Texto,
                Fecha = DateTime.UtcNow
            };

            await _repo.CrearAsync(comentario);
            await _repo.GuardarAsync();

            // ✅ Devolver el DTO recién creado
            return new ComentarioDetalleDTO
            {
                Id = comentario.Id,
                AnuncioId = comentario.AnuncioId,
                UsuarioId = comentario.UsuarioId,
                NombreUsuario = usuario.Nombre,
                Texto = comentario.Texto,
                Fecha = comentario.Fecha,
                TituloAnuncio = anuncio.Titulo
            };
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarComentarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Texto))
                return false;

            var comentario = await _repo.ObtenerPorIdAsync(id);
            if (comentario == null) return false;

            comentario.Texto = dto.Texto;
            await _repo.ActualizarAsync(comentario);
            await _repo.GuardarAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var eliminado = await _repo.EliminarPorIdAsync(id);
            if (eliminado)
                await _repo.GuardarAsync();

            return eliminado;
        }
    }
}
