using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Usuarios.DTO
{
    /// <summary>
    /// DTO utilizado para crear un nuevo anuncio con imagen.
    /// Compatible con [FromForm] para subida de archivos.
    /// </summary>
    public class CrearAnuncioDTO
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe proporcionar una imagen para el anuncio.")]
        public IFormFile Imagen { get; set; } = null!;

        public string? ImagenUrl { get; set; }

        public bool EsPasantia { get; set; } = false;
    }
}
