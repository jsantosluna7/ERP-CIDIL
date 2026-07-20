using Compras.DTO.OrdenTimelineDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Repositorios
{
    public interface IRepositorioOrdenTimeline
    {
        Task<Resultado<List<OrdenTimeline>>> OrdenTimeline();
        Task<Resultado<OrdenTimeline>> OrdenTimelineId(int id);
        Task<Resultado<List<OrdenTimeline>>> OrdenTimelinePorOrdenId(int ordenId);
    }
}