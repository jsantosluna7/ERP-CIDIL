using System;

namespace Usuarios.DTO.Comentarios
{
    public class ComentarioDetalleDTO
    {
        public int Id { get; set; }
        public int AnuncioId { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string? TituloAnuncio { get; set; } = string.Empty;
    }
}
