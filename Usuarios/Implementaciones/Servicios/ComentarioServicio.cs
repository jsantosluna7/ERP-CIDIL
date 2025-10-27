using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
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
    /// <summary>
    /// Servicio para manejar comentarios en anuncios.
    /// Solo usuarios institucionales (representados por Usuario) pueden comentar.
    /// </summary>
    public class ComentarioServicio : IComentarioServicio
    {
        private readonly IComentarioRepositorio _repo;
        private readonly IAnuncioRepositorio _anuncioRepo;
        private readonly IUsuarioRepositorio _usuarioRepo;

        public ComentarioServicio(
            IComentarioRepositorio repo,
            IAnuncioRepositorio anuncioRepo,
            IUsuarioRepositorio usuarioRepo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _anuncioRepo = anuncioRepo ?? throw new ArgumentNullException(nameof(anuncioRepo));
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
        }

        public async Task<List<ComentarioDetalleDTO>> ObtenerTodosAsync()
        {
            var comentarios = await _repo.ObtenerTodosAsync();
            return comentarios.Select(c => new ComentarioDetalleDTO
            {
                Id = c.Id,
                AnuncioId = c.AnuncioId,
                UsuarioId = c.UsuarioId,
                NombreUsuario = c.NombreUsuario ?? (c.Usuario != null ? c.Usuario.NombreUsuario : "Usuario desconocido"),
                Texto = c.Texto,
                Fecha = c.Fecha,
                TituloAnuncio = c.Anuncio?.Titulo ?? string.Empty
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
                NombreUsuario = comentario.NombreUsuario ?? (comentario.Usuario != null ? comentario.Usuario.NombreUsuario : "Usuario desconocido"),
                Texto = comentario.Texto,
                Fecha = comentario.Fecha,
                TituloAnuncio = comentario.Anuncio?.Titulo ?? string.Empty
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
                NombreUsuario = c.NombreUsuario ?? (c.Usuario != null ? c.Usuario.NombreUsuario : "Usuario desconocido"),
                Texto = c.Texto,
                Fecha = c.Fecha,
                TituloAnuncio = c.Anuncio?.Titulo ?? string.Empty
            }).ToList();
        }

        public async Task<ComentarioDetalleDTO> CrearAsync(CrearComentarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Texto))
                throw new ArgumentException("El texto del comentario no puede estar vacío.");

            var anuncio = await _anuncioRepo.ObtenerPorIdAsync(dto.AnuncioId);
            if (anuncio == null)
                throw new KeyNotFoundException($"No existe un anuncio con Id = {dto.AnuncioId}");

            var usuario = await _usuarioRepo.ObtenerPorIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new KeyNotFoundException($"No existe un usuario con Id = {dto.UsuarioId}");

            var comentario = new Comentario
            {
                AnuncioId = dto.AnuncioId,
                UsuarioId = usuario.Id,
                Texto = dto.Texto.Trim(),
                NombreUsuario = dto.NombreUsuario,
                Fecha = DateTime.UtcNow
            };

            await _repo.CrearAsync(comentario);
            await _repo.GuardarAsync();

            var comentarioConRelaciones = await _repo
                .ObtenerQueryable()
                .Include(c => c.Usuario)
                .Include(c => c.Anuncio)
                .FirstOrDefaultAsync(c => c.Id == comentario.Id);

            if (comentarioConRelaciones == null)
                throw new Exception("Error al recuperar el comentario creado.");

            return new ComentarioDetalleDTO
            {
                Id = comentarioConRelaciones.Id,
                AnuncioId = comentarioConRelaciones.AnuncioId,
                UsuarioId = comentarioConRelaciones.UsuarioId,
                NombreUsuario = comentarioConRelaciones.NombreUsuario ?? "Usuario desconocido",
                Texto = comentarioConRelaciones.Texto,
                Fecha = comentarioConRelaciones.Fecha,
                TituloAnuncio = comentarioConRelaciones.Anuncio?.Titulo ?? string.Empty
            };
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarComentarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Texto))
                return false;

            var comentario = await _repo.ObtenerPorIdAsync(id);
            if (comentario == null) return false;

            comentario.Texto = dto.Texto.Trim();
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
