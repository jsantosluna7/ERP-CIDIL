namespace Compras.DTO.EspecializadosDTO
{
    public class TimelineDTO
    {
        public DateTime? FechaEvento { get; set; }
        public string Evento { get; set; }
        public int? EstadoTimelineId { get; set; }
        public string Usuario { get; set; }
    }
}
