namespace Publicaciones.DTO.AnuncioDTO
{
    public class CrearAnuncioDTO
    {
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? ImagenUrl { get; set; }
        public bool EsPasantia { get; set; }
        public bool EsCarrusel { get; set; }
    }
}
