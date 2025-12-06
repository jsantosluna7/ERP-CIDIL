using Compras.DTO.OrdenTimelineDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Repositorios
{
    public interface IRepositorioOrdenTimeline
    {
        Task<Resultado<OrdenTimeline>> ActualizarOrdenTimeline(int id, CrearOrdenTimelineDTO ordenDTO);
        Task<Resultado<OrdenTimeline>> CrearOrdenTimeline(CrearOrdenTimelineDTO orden);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<List<OrdenTimeline>>> OrdenTimeline();
        Task<Resultado<OrdenTimeline>> OrdenTimelineId(int id);
        Task<Resultado<List<OrdenTimeline>>> OrdenTimelinePorOrdenId(int ordenId);
    }
}