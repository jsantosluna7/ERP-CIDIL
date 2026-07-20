namespace Compras.DTO.ComentariosOrdenDTO
{
    public class ComentariosOrdenDTO
    {
        public int Id { get; set; }

        public int? OrdenId { get; set; }

        public int? ItemId { get; set; }

        public int? UsuarioId { get; set; }

        public string Comentario { get; set; } = null!;
        public DateTime? CreadoEn { get; set; }
    }
}
