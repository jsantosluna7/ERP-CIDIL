namespace Usuarios.DTO.AnuncioDTO
{
    public class LikeDTO
    {
      
        // Id del anuncio al que se le da like.
        
        public int AnuncioId { get; set; }

      
        // Correo institucional del usuario que da like.
        
        public string Usuario { get; set; } = null!;
    }
}
