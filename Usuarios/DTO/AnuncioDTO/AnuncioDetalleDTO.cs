using System;

namespace Usuarios.DTO.AnuncioDTO
{
    public class AnuncioDetalleDTO
    {
        public int Id { get; set; } // Id del anuncio

        public string Titulo { get; set; } = string.Empty; // Título del anuncio

        public string Descripcion { get; set; } = string.Empty; // Descripción del anuncio

        public string? ImagenUrl { get; set; } // URL de la imagen

        public bool EsPasantia { get; set; } // Indica si es pasantía

        public bool EsCarrusel { get; set; } // Indica si va al carrusel

        public DateTime FechaPublicacion { get; set; } // Fecha de publicación

        public int UsuarioId { get; set; } // Id del usuario creador

        public string NombreUsuario { get; set; } = string.Empty; // Nombre del usuario creador
    }
}
