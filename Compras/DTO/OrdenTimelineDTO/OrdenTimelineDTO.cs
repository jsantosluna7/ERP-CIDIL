namespace Compras.DTO.OrdenTimelineDTO
{
    public class OrdenTimelineDTO
    {

        public int OrdenId { get; set; }

        public int? EstadoTimelineId { get; set; }

        public string Evento { get; set; } = null!;

        public DateTime? FechaEvento { get; set; }

        public int? CreadoPor { get; set; }
    }
}
