namespace Usuarios.DTO.NoticiasDTO
{
    public class NoticiasDTO
    {

        public string Titulo { get; set; } = null!;

        public string Categoria { get; set; } = null!;

        public string? Descripcion { get; set; }

        public string? Foto { get; set; }

        public DateTime? Fecha { get; set; }

        public bool? Propio { get; set; }

        public DateTime? CreadoEn { get; set; }
    }
}
