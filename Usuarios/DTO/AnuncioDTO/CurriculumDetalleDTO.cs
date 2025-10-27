using System;

namespace Usuarios.DTO.AnuncioDTO
{
    /// <summary>
    /// Representa la información detallada de un currículum.
    /// Incluye el anuncio asociado (si existe).
    /// </summary>
    public class CurriculumDetalleDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ArchivoUrl { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        // 🆕 Campo adicional para mostrar de qué anuncio proviene el currículum
        public string? AnuncioTitulo { get; set; } = "(Sin anuncio)";
    }
}
