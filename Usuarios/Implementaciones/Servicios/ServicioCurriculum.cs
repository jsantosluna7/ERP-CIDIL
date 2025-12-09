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
    public class ServicioCurriculum : IServicioCurriculum
    {
        private readonly IRepositorioCurriculum _repo;
        private readonly IRepositorioAnuncio _repoAnuncio;
        private readonly ILogger<ServicioCurriculum> _logger;

        public ServicioCurriculum(
            IRepositorioCurriculum repo,
            IRepositorioAnuncio repoAnuncio,
            ILogger<ServicioCurriculum> logger)
        {
            _repo = repo;
            _repoAnuncio = repoAnuncio;
            _logger = logger;
        }

        // ==================== Obtener todos los currículos ====================
        public async Task<Resultado<List<CurriculumDetalleDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var resultado = await _repo.ObtenerTodosAsync();
                if (!resultado.esExitoso || resultado.Valor == null)
                    return Resultado<List<CurriculumDetalleDTO>>.Falla("No hay currículums registrados.");

                var curriculums = resultado.Valor;

                // ⭐ SE CARGAN TODOS LOS ANUNCIOS UNA SOLA VEZ
                var anuncios = await _repoAnuncio.ObtenerTodosAsync();
                var listaAnuncios = anuncios.Valor ?? new List<Anuncio>();

                // ⭐ JOIN MANUAL (sin include)
                var lista = curriculums.Select(c =>
                {
                    // Intentamos obtener el título del campo AnuncioTitulo (guardado en la DB)
                    string tituloDB = c.AnuncioTitulo ?? "(Sin anuncio)";

                    // Si el título de la DB es el valor por defecto, buscamos el título original por ID (para backwards compatibility)
                    // Este bloque ya no debería ser necesario para nuevos registros, pero asegura compatibilidad.
                    if (tituloDB == "(Sin anuncio)" && c.AnuncioId.HasValue)
                    {
                        var anuncio = listaAnuncios.FirstOrDefault(a => a.Id == c.AnuncioId);
                        tituloDB = anuncio?.Titulo ?? "(Sin anuncio)";
                    }

                    return new CurriculumDetalleDTO
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        Email = c.Email,
                        ArchivoUrl = c.ArchivoUrl,
                        FechaEnvio = c.FechaEnvio,
                        AnuncioTitulo = tituloDB // Usamos el valor directamente del modelo si existe
                    };
                }).ToList();

                return Resultado<List<CurriculumDetalleDTO>>.Exito(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener currículums");
                return Resultado<List<CurriculumDetalleDTO>>.Falla("Error interno.");
            }
        }

        // ==================== Obtener por ID ====================
        public async Task<Resultado<CurriculumDetalleDTO?>> ObtenerPorIdAsync(int id)
        {
            try
            {
                var resultado = await _repo.ObtenerPorIdAsync(id);
                if (!resultado.esExitoso || resultado.Valor == null)
                    return Resultado<CurriculumDetalleDTO?>.Falla("Currículum no encontrado.");

                var c = resultado.Valor;

                // Intentamos obtener el título del campo AnuncioTitulo (guardado en la DB)
                string tituloDB = c.AnuncioTitulo ?? "(Sin anuncio)";

                // Si no tiene título guardado, intentamos buscarlo por ID del anuncio (para backwards compatibility)
                if (tituloDB == "(Sin anuncio)" && c.AnuncioId.HasValue)
                {
                    var anuncio = (await _repoAnuncio.ObtenerPorIdAsync(c.AnuncioId.Value)).Valor;
                    tituloDB = anuncio?.Titulo ?? "(Sin anuncio)";
                }

                var dto = new CurriculumDetalleDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Email = c.Email,
                    ArchivoUrl = c.ArchivoUrl,
                    FechaEnvio = c.FechaEnvio,
                    AnuncioTitulo = tituloDB
                };

                return Resultado<CurriculumDetalleDTO?>.Exito(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener currículum");
                return Resultado<CurriculumDetalleDTO?>.Falla("Error interno.");
            }
        }

        // ==================== Crear ====================
        public async Task<Resultado<bool>> CrearAsync(CurriculumDTO dto)
        {
            return await GuardarCurriculumAsync(dto);
        }

        public async Task<Resultado<bool>> CrearExternoAsync(CurriculumDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre) || string.IsNullOrWhiteSpace(dto.Email))
                return Resultado<bool>.Falla("Nombre y correo son obligatorios.");

            return await GuardarCurriculumAsync(dto);
        }

        private async Task<Resultado<bool>> GuardarCurriculumAsync(CurriculumDTO dto)
        {
            try
            {
                if (dto.Archivo == null || dto.Archivo.Length == 0)
                    return Resultado<bool>.Falla("Debe adjuntar un archivo PDF.");

                var extension = Path.GetExtension(dto.Archivo.FileName)?.ToLowerInvariant();
                if (extension != ".pdf")
                    return Resultado<bool>.Falla("Solo PDFs permitidos.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "curriculums");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Archivo.CopyToAsync(stream);
                }

                var curriculum = new Curriculum
                {
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    ArchivoUrl = $"/uploads/curriculums/{uniqueFileName}",
                    FechaEnvio = DateTime.UtcNow,
                    AnuncioId = dto.AnuncioId,

                    // 🚀 CORRECCIÓN CLAVE: Mapear el título del anuncio
                    // El valor recibido en el DTO (TituloAnuncio) se asigna al modelo de DB (AnuncioTitulo)
                    AnuncioTitulo = dto.TituloAnuncio
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
                return Resultado<bool>.Falla("Error interno.");
            }
        }

        public async Task<Resultado<bool>> EliminarAsync(int id)
        {
            try
            {
                var eliminado = await _repo.EliminarAsync(id);
                if (!eliminado.esExitoso)
                    return Resultado<bool>.Falla("No se pudo eliminar.");

                var guardado = await _repo.GuardarAsync();
                if (!guardado.esExitoso)
                    return Resultado<bool>.Falla("Error al guardar cambios.");

                return Resultado<bool>.Exito(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar currículum");
                return Resultado<bool>.Falla("Error interno.");
            }
        }
    }
}