using ERP.Data.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Implementaciones
{
    public class AnuncioServicio : IAnuncioServicio
    {
        private readonly IAnuncioRepositorio _repositorio;

        public AnuncioServicio(IAnuncioRepositorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        // Crear un nuevo anuncio
        public async Task CrearAsync(Anuncio anuncio)
        {
            if (anuncio == null)
                throw new ArgumentNullException(nameof(anuncio));

            await _repositorio.CrearAsync(anuncio);
            await _repositorio.GuardarAsync();
        }

        // Obtener todos los anuncios (opcionalmente filtrados por pasantías)
        public async Task<List<AnuncioDetalleDTO>> ObtenerTodosAsync(bool? esPasantia = null)
        {
            var anuncios = await _repositorio.ObtenerTodosAsync();

            if (esPasantia.HasValue)
                anuncios = anuncios.Where(a => a.EsPasantia == esPasantia.Value).ToList();

            return anuncios.Select(a => new AnuncioDetalleDTO
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Descripcion = a.Descripcion,
                ImagenUrl = a.ImagenUrl,
                EsPasantia = a.EsPasantia,
                FechaPublicacion = a.FechaPublicacion
            }).ToList();
        }

        // Obtener un anuncio por su ID
        public async Task<AnuncioDetalleDTO?> ObtenerPorIdAsync(int id)
        {
            var anuncio = await _repositorio.ObtenerPorIdAsync(id);
            if (anuncio == null)
                return null;

            return new AnuncioDetalleDTO
            {
                Id = anuncio.Id,
                Titulo = anuncio.Titulo,
                Descripcion = anuncio.Descripcion,
                ImagenUrl = anuncio.ImagenUrl,
                EsPasantia = anuncio.EsPasantia,
                FechaPublicacion = anuncio.FechaPublicacion
            };
        }

        // Actualizar un anuncio existente
        public async Task<bool> ActualizarAsync(int id, ActualizarAnuncioDTO dto)
        {
            var anuncio = await _repositorio.ObtenerPorIdAsync(id);
            if (anuncio == null)
                return false;

            anuncio.Titulo = string.IsNullOrWhiteSpace(dto.Titulo) ? anuncio.Titulo : dto.Titulo;
            anuncio.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? anuncio.Descripcion : dto.Descripcion;
            anuncio.ImagenUrl = string.IsNullOrWhiteSpace(dto.ImagenUrl) ? anuncio.ImagenUrl : dto.ImagenUrl;
            anuncio.EsPasantia = dto.EsPasantia ?? anuncio.EsPasantia;

            _repositorio.Actualizar(anuncio);
            await _repositorio.GuardarAsync();
            return true;
        }

        // Eliminar un anuncio
        public async Task<bool> EliminarAsync(int id)
        {
            var anuncio = await _repositorio.ObtenerPorIdAsync(id);
            if (anuncio == null)
                return false;

            _repositorio.Eliminar(anuncio);
            await _repositorio.GuardarAsync();
            return true;
        }

        // Obtener currículums asociados a una pasantía
        public async Task<List<string>> ObtenerCurriculumsAsync(int id)
        {
            var anuncio = await _repositorio.ObtenerPorIdAsync(id);
            if (anuncio == null || !anuncio.EsPasantia)
                return new List<string>();

            // Aquí podrías agregar lógica para devolver rutas o nombres de archivos de currículums
            return new List<string>();
        }

        // Alternar "like" de un usuario (usando string Usuario)
        public async Task<bool> ToggleLikeAsync(int anuncioId, string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El usuario no puede ser nulo o vacío.", nameof(usuario));

            // Delegamos la lógica al repositorio, que ya maneja crear o eliminar el like
            var resultado = await _repositorio.ToggleLikeAsync(anuncioId, usuario);
            return resultado;
        }
    }
}
