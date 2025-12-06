using Compras.DTO.ComentariosOrdenDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Repositorios
{
    public interface IRepositorioComentariosOrden
    {
        Task<Resultado<ComentariosOrden>> ActualizarComentariosOrden(int id, CrearComentariosOrdenDTO comentarioDTO);
        Task<Resultado<List<ComentariosOrden>>> ComentariosOrden();
        Task<Resultado<ComentariosOrden>> ComentariosOrdenId(int id);
        Task<Resultado<List<ComentariosOrden>>> ComentariosPorItemId(int itemId);
        Task<Resultado<List<ComentariosOrden>>> ComentariosPorOrdenId(int ordenId);
        Task<Resultado<List<ComentariosOrden>>> ComentariosPorUsuarioId(int usuarioId);
        Task<Resultado<ComentariosOrden>> CrearComentariosOrden(CrearComentariosOrdenDTO comentario);
        Task<Resultado<bool?>> Eliminar(int id);
    }
}