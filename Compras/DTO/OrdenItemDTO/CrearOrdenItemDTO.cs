namespace Compras.DTO.OrdenItemDTO
{
    public class CrearOrdenItemDTO
    {
        public int OrdenId { get; set; }

        public string? NumeroLista { get; set; }

        public string Nombre { get; set; } = null!;

        public int Cantidad { get; set; }

        public int? CantidadRecibida { get; set; }

        public string? Comentario { get; set; }
    }
}
