namespace Compras.DTO.OrdenItemDTO
{
    public class OrdenItemDTO
    {
        public int OrdenId { get; set; }

        public string? NumeroLista { get; set; }

        public string Nombre { get; set; } = null!;

        public int? EstadoTimelineId { get; set; }

        public int Cantidad { get; set; }

        public int? CantidadRecibida { get; set; }

        public string? Comentario { get; set; }

        public DateTime? ActualizadoEn { get; set; }
    }
}
