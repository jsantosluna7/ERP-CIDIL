namespace Usuarios.DTO.AnuncioDTO
{
    public class LikeDTO
    {
        /// <summary>
        /// Id del anuncio al que se le da like.
        /// </summary>
        public int AnuncioId { get; set; }

        /// <summary>
        /// Correo institucional del usuario que da like.
        /// </summary>
        public string Usuario { get; set; } = null!;
    }
}
