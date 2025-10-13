namespace Usuarios.DTO.AnuncioDTO
{
    public class LikeDTO
    {
        public int AnuncioId { get; set; }

        // Puedes enviar el nombre, correo o Id
        public int? UsuarioId { get; set; }
        public string? Usuario { get; set; }
    }
}
