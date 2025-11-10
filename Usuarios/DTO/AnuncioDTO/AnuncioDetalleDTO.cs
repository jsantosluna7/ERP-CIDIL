using System;

namespace Usuarios.DTO.AnuncioDTO
{
    // He ajustado el namespace a DTO.AnuncioDTO, asumiendo la convención. 
    // Si tu archivo está solo en Usuarios.DTO, ajústalo.
    public class AnuncioDetalleDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public bool EsPasantia { get; set; }
        public DateTime FechaPublicacion { get; set; }

        // 👇 Clave Foránea del creador
        public int UsuarioId { get; set; }

        // ✅ CORRECCIÓN CLAVE: Propiedad para mostrar el nombre completo
        public string NombreUsuario { get; set; } = string.Empty;
    }
}