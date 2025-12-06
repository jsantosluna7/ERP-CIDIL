using Compras.DTO.EstadosTimelineDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Repositorios
{
    public interface IRepositorioEstadosTimeline
    {
        Task<Resultado<EstadosTimeline>> ActualizarEstadosTimeline(int id, EstadosTimelineDTO estadoDTO);
        Task<Resultado<EstadosTimeline>> CrearEstadosTimeline(EstadosTimelineDTO estado);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<List<EstadosTimeline>>> EstadosTimeline();
        Task<Resultado<EstadosTimeline>> EstadosTimelineId(int id);
    }
}