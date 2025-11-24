using System;

namespace Usuarios.DTO.AnuncioDTO
{
    public class AnuncioDetalleDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public string? ImagenUrl { get; set; }

        public bool EsPasantia { get; set; }

        // NUEVA propiedad para indicar si va al carrusel
        public bool EsCarrusel { get; set; }

        public DateTime FechaPublicacion { get; set; }

        // Clave foránea del usuario creador
        public int UsuarioId { get; set; }

        // Nombre completo del usuario
        public string NombreUsuario { get; set; } = string.Empty;
    }
}
