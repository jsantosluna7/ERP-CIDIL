namespace Usuarios.DTO.Comentarios
{
    
    // DTO para crear un comentario asociado a un anuncio
  
    public class ComentarioDTO
    {
        //del anuncio al que pertenece el comentario
        public int AnuncioId { get; set; }

        //del usuario que comenta
        public int UsuarioId { get; set; }

        // Texto del comentario
        public string Texto { get; set; } = string.Empty;
    }
}
