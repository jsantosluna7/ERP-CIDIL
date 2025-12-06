using Compras.DTO.ComentariosOrdenDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Servicios
{
    public interface IServicioComentariosOrden
    {
        Task<Resultado<ComentariosOrdenDTO>> ActualizarComentariosOrden(int id, CrearComentariosOrdenDTO comentariosDTO);
        Task<Resultado<List<ComentariosOrden>>> ComentariosOrden();
        Task<Resultado<ComentariosOrden>> ComentariosOrdenId(int id);
        Task<Resultado<List<ComentariosOrden>>> ComentariosOrdenPorItemId(int itemId);
        Task<Resultado<List<ComentariosOrden>>> ComentariosOrdenPorOrdenId(int ordenId);
        Task<Resultado<List<ComentariosOrden>>> ComentariosOrdenPorUsuarioId(int usuarioId);
        Task<Resultado<ComentariosOrdenDTO>> CrearComentariosOrden(CrearComentariosOrdenDTO comentarios);
        Task<Resultado<bool?>> Eliminar(int id);
    }
}