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
    
    // Servicio para manejar "Likes" en anuncios.
    // Solo usuarios institucionales pueden dar Like.
   
    public class ServicioLike : ILikeServicio
    {
        private readonly ILikeRepositorio _repo;
        private readonly IUsuarioRepositorio _usuarioRepo;

        public ServicioLike(ILikeRepositorio repo, IUsuarioRepositorio usuarioRepo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _usuarioRepo = usuarioRepo ?? throw new ArgumentNullException(nameof(usuarioRepo));
        }

        // ==================== Obtener todos los likes ====================
        public async Task<List<LikeDTO>> ObtenerTodosAsync()
        {
            var resultado = await _repo.ObtenerTodosAsync();
            if (resultado == null || !resultado.esExitoso || resultado.Valor == null)
                return new List<LikeDTO>();

            return resultado.Valor.Select(l => new LikeDTO
            {
                AnuncioId = l.AnuncioId,
                Usuario = l.Usuario?.CorreoInstitucional ?? "Desconocido"
            }).ToList();
        }

        // ==================== Obtener like por Id ====================
        public async Task<LikeDTO?> ObtenerPorIdAsync(int id)
        {
            var resultado = await _repo.ObtenerPorIdAsync(id);
            if (resultado == null || !resultado.esExitoso || resultado.Valor == null)
                return null;

            var like = resultado.Valor;
            return new LikeDTO
            {
                AnuncioId = like.AnuncioId,
                Usuario = like.Usuario?.CorreoInstitucional ?? "Desconocido"
            };
        }

        // ==================== Crear / Alternar like ====================
        public async Task<(bool estadoActual, int totalLikes)> CrearAsync(LikeDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Usuario))
                throw new ArgumentException("Usuario obligatorio", nameof(dto.Usuario));

            var usuario = await _usuarioRepo.ObtenerPorCorreoAsync(dto.Usuario);
            if (usuario == null)
                throw new InvalidOperationException($"No existe un usuario con el correo '{dto.Usuario}'.");

            var existente = await _repo.ObtenerPorAnuncioYUsuarioAsync(dto.AnuncioId, usuario.CorreoInstitucional);
            bool estadoActual;

            if (existente != null && existente.esExitoso && existente.Valor != null)
            {
                // Ya existe → eliminar
                var eliminado = await _repo.EliminarAsync(existente.Valor.Id);
                if (eliminado == null || !eliminado.esExitoso || !eliminado.Valor)
                    throw new InvalidOperationException("Error al eliminar like existente.");

                estadoActual = false;
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
                var creado = await _repo.CrearAsync(nuevoLike);
                if (creado == null || !creado.esExitoso || !creado.Valor)
                    throw new InvalidOperationException("Error al crear like.");

                estadoActual = true;
            }

            var totalResultado = await _repo.ContarPorAnuncioAsync(dto.AnuncioId);
            if (totalResultado == null || !totalResultado.esExitoso)
                throw new InvalidOperationException("Error al contar likes.");

            return (estadoActual, totalResultado.Valor);
        }

        // ==================== Eliminar like ====================
        public async Task<bool> EliminarAsync(int id)
        {
            var resultado = await _repo.EliminarAsync(id);
            return resultado != null && resultado.esExitoso && resultado.Valor;
        }

        // ==================== Contar likes por anuncio ====================
        public async Task<int> ContarPorAnuncioAsync(int anuncioId)
        {
            var resultado = await _repo.ContarPorAnuncioAsync(anuncioId);
            return resultado != null && resultado.esExitoso ? resultado.Valor : 0;
        }

        // ==================== Verificar existencia de like ====================
        public async Task<bool> ExisteLikeAsync(int anuncioId, string usuarioCorreo)
        {
            var usuario = await _usuarioRepo.ObtenerPorCorreoAsync(usuarioCorreo);
            if (usuario == null) return false;

            var existente = await _repo.ObtenerPorAnuncioYUsuarioAsync(anuncioId, usuario.CorreoInstitucional);
            return existente != null && existente.esExitoso && existente.Valor != null;
        }
    }
}
