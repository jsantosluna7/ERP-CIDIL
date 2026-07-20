namespace Compras.DTO.EstadosTimelineDTO
{
    public class EstadosTimelineDTO
    {
        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string? Color { get; set; }

        public string? Icono { get; set; }

        public bool? Activo { get; set; }
    }
}
