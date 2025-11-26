using Microsoft.AspNetCore.Http;

namespace Usuarios.DTO
{
    
    // DTO para actualizar un anuncio existente.

    public class ActualizarAnuncioDTO
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public bool? EsPasantia { get; set; }
        public IFormFile? Imagen { get; set; }
        public string? ImagenUrl { get; set; }

        //Permite activar/desactivar carrusel en edición
        public bool? EsCarrusel { get; set; }
    }
}
