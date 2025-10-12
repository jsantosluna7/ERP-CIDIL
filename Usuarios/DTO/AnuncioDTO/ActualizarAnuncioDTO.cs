using Microsoft.AspNetCore.Http;

namespace Usuarios.DTO
{
    /// <summary>
    /// DTO para actualizar un anuncio existente.
    /// </summary>
    public class ActualizarAnuncioDTO
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public bool? EsPasantia { get; set; }
        public IFormFile? Imagen { get; set; }
        public string? ImagenUrl { get; set; }
    }
}
