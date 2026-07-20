using Compras.DTO.OrdenTimelineDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Servicios
{
    public interface IServicioOrdenTimeline
    {
        Task<Resultado<List<OrdenTimeline>>> OrdenTimeline();
        Task<Resultado<OrdenTimeline>> OrdenTimelineId(int id);
        Task<Resultado<List<OrdenTimeline>>> OrdenTimelinePorOrdenId(int ordenId);
    }
}