namespace Usuarios.DTO.Comentarios
{
    /// <summary>
    /// DTO para crear un comentario asociado a un anuncio
    /// </summary>
    public class ComentarioDTO
    {
        // Id del anuncio al que pertenece el comentario
        public int AnuncioId { get; set; }

        // Id del usuario que comenta
        public int UsuarioId { get; set; }

        // Texto del comentario
        public string Texto { get; set; } = string.Empty;
    }
}
