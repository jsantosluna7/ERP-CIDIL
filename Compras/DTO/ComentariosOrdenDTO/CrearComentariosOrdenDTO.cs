namespace Compras.DTO.ComentariosOrdenDTO
{
    public class CrearComentariosOrdenDTO
    {

        public int? OrdenId { get; set; }

        public int? ItemId { get; set; }

        public int? UsuarioId { get; set; }

        public string Comentario { get; set; } = null!;
    }
}
