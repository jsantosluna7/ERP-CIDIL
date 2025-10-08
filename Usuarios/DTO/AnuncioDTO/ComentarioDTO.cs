namespace Usuarios.DTO.Comentarios
{
    public class ComentarioDTO
    {
        public int AnuncioId { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
    }
}
