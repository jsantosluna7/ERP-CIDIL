using System;

namespace Usuarios.DTO.Comentarios
{
    public class ComentarioDetalleDTO
    {
        public int Id { get; set; }
        public int AnuncioId { get; set; }

        // ID del usuario que comentó
        public int UsuarioId { get; set; }

        // Nombre del usuario que comentó
        public string NombreUsuario { get; set; } = string.Empty;

        public string Texto { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }

        // Título del anuncio al que pertenece el comentario
        public string? TituloAnuncio { get; set; } = string.Empty;
    }
}
