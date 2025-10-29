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
        private readonly IComentarioRepositorio _comentarioRepo;
        private readonly IAnuncioRepositorio _anuncioRepo;
        private readonly IUsuarioRepositorio _usuarioRepo;

        public ComentarioServicio(
            IComentarioRepositorio comentarioRepo,
            IAnuncioRepositorio anuncioRepo,
            IUsuarioRepositorio usuarioRepo)
        {
            _comentarioRepo = comentarioRepo ?? throw new ArgumentNullException(nameof(comentarioRepo));
            _anuncioRepo = anuncioRepo ?? throw new ArgumentNullException(nameof(anuncioRepo));
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
        }

        public async Task<Resultado<List<ComentarioDetalleDTO>>> ObtenerTodosAsync()
        {
            var comentariosResultado = await _comentarioRepo.ObtenerTodosAsync();

            if (!comentariosResultado.esExitoso)
                return Resultado<List<ComentarioDetalleDTO>>.Falla("No se pudieron obtener los comentarios.");

            var comentarios = comentariosResultado.Valor;

            if (comentarios == null || !comentarios.Any())
                return Resultado<List<ComentarioDetalleDTO>>.Falla("No hay comentarios registrados.");

            var lista = comentarios.Select(c => new ComentarioDetalleDTO
            {
                Id = c.Id,
                AnuncioId = c.AnuncioId,
                UsuarioId = c.UsuarioId,
                NombreUsuario = c.NombreUsuario ?? "Usuario desconocido",
                Texto = c.Texto,
                Fecha = c.Fecha,
                TituloAnuncio = string.Empty 
            }).ToList();

            return Resultado<List<ComentarioDetalleDTO>>.Exito(lista);
        }

        public async Task<Resultado<ComentarioDetalleDTO>> ObtenerPorIdAsync(int id)
        {
            var comentarioResultado = await _comentarioRepo.ObtenerPorIdAsync(id);

            if (!comentarioResultado.esExitoso || comentarioResultado.Valor == null)
                return Resultado<ComentarioDetalleDTO>.Falla($"No se encontró un comentario con Id = {id}.");

            var c = comentarioResultado.Valor;

            var dto = new ComentarioDetalleDTO
            {
                Id = c.Id,
                AnuncioId = c.AnuncioId,
                UsuarioId = c.UsuarioId,
                NombreUsuario = c.NombreUsuario ?? "Usuario desconocido",
                Texto = c.Texto,
                Fecha = c.Fecha,
                TituloAnuncio = string.Empty 
            };

            return Resultado<ComentarioDetalleDTO>.Exito(dto);
        }

        public async Task<Resultado<List<ComentarioDetalleDTO>>> ObtenerPorAnuncioIdAsync(int anuncioId)
        {
            var comentariosResultado = await _comentarioRepo.ObtenerPorAnuncioAsync(anuncioId);

            if (!comentariosResultado.esExitoso)
                return Resultado<List<ComentarioDetalleDTO>>.Falla("No se pudieron obtener los comentarios.");

            var comentarios = comentariosResultado.Valor;

            if (comentarios == null || !comentarios.Any())
                return Resultado<List<ComentarioDetalleDTO>>.Falla("No hay comentarios para este anuncio.");

            var lista = comentarios.Select(c => new ComentarioDetalleDTO
            {
                Id = c.Id,
                AnuncioId = c.AnuncioId,
                UsuarioId = c.UsuarioId,
                NombreUsuario = c.NombreUsuario ?? "Usuario desconocido",
                Texto = c.Texto,
                Fecha = c.Fecha,
                TituloAnuncio = string.Empty 
            }).ToList();

            return Resultado<List<ComentarioDetalleDTO>>.Exito(lista);
        }

        public async Task<Resultado<ComentarioDetalleDTO>> CrearAsync(CrearComentarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Texto))
                return Resultado<ComentarioDetalleDTO>.Falla("El texto del comentario no puede estar vacío.");

            var anuncioResultado = await _anuncioRepo.ObtenerPorIdAsync(dto.AnuncioId);
            if (!anuncioResultado.esExitoso || anuncioResultado.Valor == null)
                return Resultado<ComentarioDetalleDTO>.Falla($"No existe un anuncio con Id = {dto.AnuncioId}.");

            var usuarioResultado = await _usuarioRepo.ObtenerPorIdAsync(dto.UsuarioId);
            if (usuarioResultado == null)
                return Resultado<ComentarioDetalleDTO>.Falla($"No existe un usuario con Id = {dto.UsuarioId}.");

            var comentario = new Comentario
            {
                AnuncioId = dto.AnuncioId,
                UsuarioId = usuarioResultado.Id,
                Texto = dto.Texto.Trim(),
                NombreUsuario = dto.NombreUsuario,
                Fecha = DateTime.UtcNow
            };

            var crearResultado = await _comentarioRepo.CrearAsync(comentario);
            if (!crearResultado.esExitoso)
                return Resultado<ComentarioDetalleDTO>.Falla("Error al crear el comentario.");

            var nuevoComentarioResultado = await _comentarioRepo.ObtenerPorIdAsync(comentario.Id);
            if (!nuevoComentarioResultado.esExitoso || nuevoComentarioResultado.Valor == null)
                return Resultado<ComentarioDetalleDTO>.Falla("Error al recuperar el comentario creado.");

            var dtoFinal = new ComentarioDetalleDTO
            {
                Id = nuevoComentarioResultado.Valor.Id,
                AnuncioId = nuevoComentarioResultado.Valor.AnuncioId,
                UsuarioId = nuevoComentarioResultado.Valor.UsuarioId,
                NombreUsuario = nuevoComentarioResultado.Valor.NombreUsuario ?? "Usuario desconocido",
                Texto = nuevoComentarioResultado.Valor.Texto,
                Fecha = nuevoComentarioResultado.Valor.Fecha,
                TituloAnuncio = anuncioResultado.Valor.Titulo
            };

            return Resultado<ComentarioDetalleDTO>.Exito(dtoFinal);
        }

        public async Task<Resultado<bool>> ActualizarAsync(int id, ActualizarComentarioDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Texto))
                return Resultado<bool>.Falla("El texto no puede estar vacío.");

            var comentarioResultado = await _comentarioRepo.ObtenerPorIdAsync(id);
            if (!comentarioResultado.esExitoso || comentarioResultado.Valor == null)
                return Resultado<bool>.Falla($"No se encontró un comentario con Id = {id}.");

            var comentario = comentarioResultado.Valor;
            comentario.Texto = dto.Texto.Trim();

            var actualizarResultado = await _comentarioRepo.ActualizarAsync(comentario);
            if (!actualizarResultado.esExitoso)
                return Resultado<bool>.Falla("Error al actualizar el comentario.");

            return Resultado<bool>.Exito(true);
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            var eliminarResultado = await _comentarioRepo.EliminarPorIdAsync(id);
            if (!eliminarResultado.esExitoso)
                return Resultado<bool>.Falla("No se pudo eliminar el comentario.");

            return Resultado<bool>.Exito(true);
        }
    }
}

