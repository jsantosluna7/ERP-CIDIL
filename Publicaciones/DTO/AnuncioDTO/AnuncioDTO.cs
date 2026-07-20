namespace Publicaciones.DTO.AnuncioDTO
{
    public class AnuncioDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? ImagenUrl { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public bool? EsPasantia { get; set; }
        public bool EsCarrusel { get; set; }
    }
}
