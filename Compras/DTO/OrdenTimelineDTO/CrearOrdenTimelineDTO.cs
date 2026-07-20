namespace Compras.DTO.OrdenTimelineDTO
{
    public class CrearOrdenTimelineDTO
    {

        public int OrdenId { get; set; }

        public int? EstadoTimelineId { get; set; }

        public string Evento { get; set; } = null!;

        public int? CreadoPor { get; set; }
    }
}
