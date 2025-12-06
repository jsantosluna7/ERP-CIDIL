using Compras.DTO.OrdenTimelineDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Servicios
{
    public interface IServicioOrdenTimeline
    {
        Task<Resultado<OrdenTimelineDTO>> ActualizarOrdenTimeline(int id, CrearOrdenTimelineDTO ordenTimelineDTO);
        Task<Resultado<OrdenTimelineDTO>> CrearOrdenTimeline(CrearOrdenTimelineDTO ordenTimeline);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<List<OrdenTimeline>>> OrdenTimeline();
        Task<Resultado<OrdenTimeline>> OrdenTimelineId(int id);
        Task<Resultado<List<OrdenTimeline>>> OrdenTimelinePorOrdenId(int ordenId);
    }
}