namespace Usuarios.DTO.AnuncioDTO
{
    public class CurriculumDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IFormFile Archivo { get; set; } = default!;
        public int? AnuncioId { get; set; } //Para saber de qué anuncio viene el currículum
    }
}
