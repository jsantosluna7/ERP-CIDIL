using System;

namespace Usuarios.DTO
{
    public class AnuncioDetalleDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public bool EsPasantia { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
