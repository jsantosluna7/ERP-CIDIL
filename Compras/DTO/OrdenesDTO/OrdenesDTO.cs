namespace Compras.DTO.OrdenesDTO
{
    public class OrdenesDTO
    {

        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string? Departamento { get; set; }

        public string? UnidadNegocio { get; set; }

        public string? SolicitadoPor { get; set; }

        public DateOnly? FechaSolicitud { get; set; }

        public DateOnly? FechaSubida { get; set; }

        public string? Moneda { get; set; }

        public decimal? ImporteTotal { get; set; }

        public string? Comentario { get; set; }

        public int? EstadoTimelineId { get; set; }

        public int? CreadoPor { get; set; }

        public DateTime? ActualizadoEn { get; set; }
    }
}
