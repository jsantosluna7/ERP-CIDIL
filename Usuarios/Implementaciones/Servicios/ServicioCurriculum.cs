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
   
    // Servicio encargado de la gestión de currículums
    // usando el patrón Resultado<T> para manejo de errores y respuestas.
    
    public class ServicioCurriculum : IServicioCurriculum
    {
        private readonly IRepositorioCurriculum _repo;
        private readonly ILogger<ServicioCurriculum> _logger;

        public ServicioCurriculum(IRepositorioCurriculum repo, ILogger<ServicioCurriculum> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // ==================== Obtener todos los currículos ====================
        public async Task<Resultado<List<CurriculumDetalleDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var resultado = await _repo.ObtenerTodosAsync();
                if (!resultado.esExitoso || resultado.Valor == null || resultado.Valor.Count == 0)
                    return Resultado<List<CurriculumDetalleDTO>>.Falla("No hay currículums registrados.");

                var lista = resultado.Valor.Select(c => new CurriculumDetalleDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Email = c.Email,
                    ArchivoUrl = c.ArchivoUrl,
                    FechaEnvio = c.FechaEnvio,
                    AnuncioTitulo = c.Anuncio != null ? c.Anuncio.Titulo : "(Sin anuncio)"
                }).ToList();

                return Resultado<List<CurriculumDetalleDTO>>.Exito(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los currículums");
                return Resultado<List<CurriculumDetalleDTO>>.Falla("Ocurrió un error al obtener los currículums.");
            }
        }

        // ==================== Obtener currículum por ID ====================
        public async Task<Resultado<CurriculumDetalleDTO?>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var resultado = await _repo.ObtenerPorIdAsync(id);
                if (!resultado.esExitoso || resultado.Valor == null)
                    return Resultado<CurriculumDetalleDTO?>.Falla($"No se encontró el currículum con ID {id}.");

                var c = resultado.Valor;
                var dto = new CurriculumDetalleDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Email = c.Email,
                    ArchivoUrl = c.ArchivoUrl,
                    FechaEnvio = c.FechaEnvio,
                    AnuncioTitulo = c.Anuncio != null ? c.Anuncio.Titulo : "(Sin anuncio)"
                };

                return Resultado<CurriculumDetalleDTO?>.Exito(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener currículum con ID {id}");
                return Resultado<CurriculumDetalleDTO?>.Falla("Ocurrió un error al obtener el currículum.");
            }
        }

        // ==================== Crear currículum (usuarios autenticados) ====================
        public async Task<Resultado<bool>> CrearAsync(CurriculumDTO dto)
        {
            return await GuardarCurriculumAsync(dto);
        }

        // ==================== Crear currículum externo (usuarios sin sesión) ====================
        public async Task<Resultado<bool>> CrearExternoAsync(CurriculumDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Email))
                return Resultado<bool>.Falla("Nombre y correo son obligatorios para currículum externo.");

            return await GuardarCurriculumAsync(dto);
        }

        // ==================== Método privado para guardar currículum ====================
        private async Task<Resultado<bool>> GuardarCurriculumAsync(CurriculumDTO dto)
        {
            try
            {
                if (dto.Archivo == null || dto.Archivo.Length == 0)
                    return Resultado<bool>.Falla("Debe adjuntar un archivo PDF.");

                var extension = Path.GetExtension(dto.Archivo.FileName)?.ToLowerInvariant();
                if (extension != ".pdf")
                    return Resultado<bool>.Falla("Solo se permiten archivos en formato PDF.");

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

                var creado = await _repo.CrearAsync(curriculum);
                if (!creado.esExitoso)
                    return Resultado<bool>.Falla(creado.MensajeError);

                var guardado = await _repo.GuardarAsync();
                if (!guardado.esExitoso)
                    return Resultado<bool>.Falla(guardado.MensajeError);

                return Resultado<bool>.Exito(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar currículum");
                return Resultado<bool>.Falla("Ocurrió un error al guardar el currículum. Intente nuevamente.");
            }
        }

        // ==================== Eliminar currículum ====================
        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var eliminado = await _repo.EliminarAsync(id);
                if (!eliminado.esExitoso || !eliminado.Valor)
                    return Resultado<bool>.Falla("No se pudo eliminar el currículum.");

                var guardado = await _repo.GuardarAsync();
                if (!guardado.esExitoso)
                    return Resultado<bool>.Falla("Error al guardar cambios en la base de datos.");

                return Resultado<bool>.Exito(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar currículum con ID {id}");
                return Resultado<bool>.Falla("Ocurrió un error al eliminar el currículum.");
            }
        }
    }
}
