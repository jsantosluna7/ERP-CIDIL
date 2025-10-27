using ERP.Data.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Implementaciones.Servicios
{
    public class CurriculumServicio : ICurriculumServicio
    {
        private readonly ICurriculumRepositorio _repo;
        private readonly ILogger<CurriculumServicio> _logger;

        public CurriculumServicio(ICurriculumRepositorio repo, ILogger<CurriculumServicio> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // ✅ Obtener todos los currículos
        public async Task<List<CurriculumDetalleDTO>> ObtenerTodosAsync()
        {
            try
            {
                var curriculums = await _repo.ObtenerTodosAsync();
                return curriculums.Select(c => new CurriculumDetalleDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Email = c.Email,
                    ArchivoUrl = c.ArchivoUrl,
                    FechaEnvio = c.FechaEnvio,
                    AnuncioTitulo = c.Anuncio != null ? c.Anuncio.Titulo : "(Sin anuncio)"
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los currículums");
                return new List<CurriculumDetalleDTO>();
            }
        }

        // ✅ Obtener currículum por ID
        public async Task<CurriculumDetalleDTO?> ObtenerPorIdAsync(int id)
        {
            try
            {
                var c = await _repo.ObtenerPorIdAsync(id);
                if (c == null)
                    return null;

                return new CurriculumDetalleDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Email = c.Email,
                    ArchivoUrl = c.ArchivoUrl,
                    FechaEnvio = c.FechaEnvio,
                    AnuncioTitulo = c.Anuncio != null ? c.Anuncio.Titulo : "(Sin anuncio)"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener currículum con ID {id}");
                return null;
            }
        }

        // ✅ Crear currículum (solo archivo PDF)
        public async Task CrearAsync(CurriculumDTO dto)
        {
            try
            {
                if (dto.Archivo == null || dto.Archivo.Length == 0)
                    throw new InvalidOperationException("Debe adjuntar un archivo PDF.");

                var extension = Path.GetExtension(dto.Archivo.FileName)?.ToLowerInvariant();
                if (extension != ".pdf")
                    throw new InvalidOperationException("Solo se permiten archivos en formato PDF.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "curriculums");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Archivo.CopyToAsync(stream);
                }

                string archivoUrl = $"/uploads/curriculums/{uniqueFileName}";

                var curriculum = new Curriculum
                {
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    ArchivoUrl = archivoUrl,
                    FechaEnvio = DateTime.UtcNow,
                    AnuncioId = dto.AnuncioId
                };

                await _repo.CrearAsync(curriculum);
                await _repo.GuardarAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar currículum");
                throw new InvalidOperationException("Ocurrió un error al guardar el currículum. Intente nuevamente.");
            }
        }

        // ✅ Eliminar currículum
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var eliminado = await _repo.EliminarAsync(id);
                await _repo.GuardarAsync();
                return eliminado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar currículum con ID {id}");
                return false;
            }
        }
    }
}
