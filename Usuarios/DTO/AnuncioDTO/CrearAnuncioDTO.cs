using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Usuarios.DTO.AnuncioDTO
{
    /// <summary>
    /// DTO utilizado para crear un nuevo anuncio con imagen.
    /// Compatible con [FromForm] para subida de archivos.
    /// </summary>
    public class CrearAnuncioDTO
    {
        /// <summary>
        /// Título del anuncio.
        /// </summary>
        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del anuncio.
        /// </summary>
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Imagen física subida para el anuncio.
        /// Debe enviarse como archivo en formato multipart/form-data.
        /// Esta propiedad es obligatoria.
        /// </summary>
        [Required(ErrorMessage = "Debe proporcionar una imagen para el anuncio.")]
        public IFormFile Imagen { get; set; } = null!;
    }
}
