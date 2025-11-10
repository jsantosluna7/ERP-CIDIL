using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Usuarios.DTO
{
    
    // DTO utilizado para crear un nuevo anuncio con varias imágenes.
    // Compatible con [FromForm] para subida de archivos.

    public class CrearAnuncioDTO
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe proporcionar al menos una imagen para el anuncio.")]
        public IFormFile[] Imagenes { get; set; } = Array.Empty<IFormFile>();

        public bool EsPasantia { get; set; } = false;

        
    }
}
