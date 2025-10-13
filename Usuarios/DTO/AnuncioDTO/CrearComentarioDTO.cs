namespace Usuarios.DTO.Comentarios
{
    public class CrearComentarioDTO
    {
        public int AnuncioId { get; set; }

        // ID del usuario registrado que comenta
        public int UsuarioId { get; set; }

        public string Texto { get; set; } = string.Empty;
    }
}
