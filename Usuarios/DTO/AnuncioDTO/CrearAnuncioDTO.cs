namespace Usuarios.DTO.AnuncioDTO
{
    // DTO para la creación de un anuncio
    public class CrearAnuncioDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
    }
}
