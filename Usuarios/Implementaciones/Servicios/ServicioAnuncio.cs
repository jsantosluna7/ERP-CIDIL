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
    public class ServicioAnuncio : IAnuncioServicio
    {
        private readonly IAnuncioRepositorio _repositorio;

        public ServicioAnuncio(IAnuncioRepositorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        // Crear un nuevo anuncio
        public async Task<Resultado<bool>> CrearAsync(Anuncio anuncio)
        {
            if (anuncio == null)
                return Resultado<bool>.Falla("El anuncio no puede ser nulo.");

            var resultado = await _repositorio.CrearAsync(anuncio);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "Error desconocido");

            await _repositorio.GuardarAsync();
            return Resultado<bool>.Exito(true);
        }

        // Obtener todos los anuncios (opcionalmente filtrados por pasantías)
        public async Task<Resultado<List<AnuncioDetalleDTO>>> ObtenerTodosAsync(bool? esPasantia = null)
        {
            var resultado = await _repositorio.ObtenerTodosAsync();
            if (!resultado.esExitoso)
                return Resultado<List<AnuncioDetalleDTO>>.Falla(resultado.MensajeError ?? "Error desconocido");

            var anuncios = resultado.Valor ?? new List<Anuncio>();

            if (esPasantia.HasValue)
                anuncios = anuncios.Where(a => a.EsPasantia == esPasantia.Value).ToList();

            var dtos = anuncios.Select(a => new AnuncioDetalleDTO
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Descripcion = a.Descripcion,
                ImagenUrl = a.ImagenUrl,
                EsPasantia = a.EsPasantia,
                FechaPublicacion = a.FechaPublicacion
            }).ToList();

            return Resultado<List<AnuncioDetalleDTO>>.Exito(dtos);
        }

        // Obtener un anuncio por su ID
        public async Task<Resultado<AnuncioDetalleDTO>> ObtenerPorIdAsync(int id)
        {
            var resultado = await _repositorio.ObtenerPorIdAsync(id);
            if (!resultado.esExitoso)
                return Resultado<AnuncioDetalleDTO>.Falla(resultado.MensajeError ?? "Error desconocido");

            var a = resultado.Valor!;
            var dto = new AnuncioDetalleDTO
            {
                Id = a.Id,
                Titulo = a.Titulo,
                Descripcion = a.Descripcion,
                ImagenUrl = a.ImagenUrl,
                EsPasantia = a.EsPasantia,
                FechaPublicacion = a.FechaPublicacion
            };

            return Resultado<AnuncioDetalleDTO>.Exito(dto);
        }

        // Actualizar un anuncio existente
        public async Task<Resultado<bool>> ActualizarAsync(int id, ActualizarAnuncioDTO dto)
        {
            var resultado = await _repositorio.ObtenerPorIdAsync(id);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "Error desconocido");

            var anuncio = resultado.Valor!;
            anuncio.Titulo = string.IsNullOrWhiteSpace(dto.Titulo) ? anuncio.Titulo : dto.Titulo;
            anuncio.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? anuncio.Descripcion : dto.Descripcion;
            anuncio.ImagenUrl = string.IsNullOrWhiteSpace(dto.ImagenUrl) ? anuncio.ImagenUrl : dto.ImagenUrl;
            anuncio.EsPasantia = dto.EsPasantia ?? anuncio.EsPasantia;

            var resActualiza = await _repositorio.ActualizarAsync(anuncio);
            if (!resActualiza.esExitoso)
                return Resultado<bool>.Falla(resActualiza.MensajeError ?? "Error desconocido");

            await _repositorio.GuardarAsync();
            return Resultado<bool>.Exito(true);
        }

        // Eliminar un anuncio
        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            var resultado = await _repositorio.EliminarAsync(id);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "Error desconocido");

            return Resultado<bool>.Exito(true);
        }

        // Obtener currículums asociados a un anuncio
        public async Task<Resultado<List<string>>> ObtenerCurriculumsAsync(int id)
        {
            var resultado = await _repositorio.ObtenerCurriculumsAsync(id);
            if (!resultado.esExitoso)
                return Resultado<List<string>>.Falla(resultado.MensajeError ?? "Error desconocido");

            var lista = resultado.Valor!
                .Select(c => c.ArchivoUrl)
                .ToList();

            return Resultado<List<string>>.Exito(lista);
        }

        // Guardar currículum externo
        public async Task<Resultado<bool>> GuardarCurriculumAsync(int anuncioId, string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo))
                return Resultado<bool>.Falla("El archivo no puede estar vacío.");

            var curriculum = new Curriculum
            {
                AnuncioId = anuncioId,
                Nombre = "Externo",
                Email = "",
                ArchivoUrl = nombreArchivo,
                FechaEnvio = DateTime.UtcNow,
                EsExterno = true
            };

            var resultado = await _repositorio.AgregarCurriculumAsync(curriculum);
            if (!resultado.esExitoso)
                return Resultado<bool>.Falla(resultado.MensajeError ?? "Error al guardar currículum");

            await _repositorio.GuardarAsync();
            return Resultado<bool>.Exito(true);
        }

        // Alternar "like" de un usuario usando su Id
        public async Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId)
        {
            var resultado = await _repositorio.ToggleLikeAsync(anuncioId, usuarioId);
            return resultado;
        }
    }
}
