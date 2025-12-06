using Compras.DTO.EstadosTimelineDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Servicios
{
    public interface IServicioEstadosTimeline
    {
        Task<Resultado<EstadosTimelineDTO>> ActualizarEstadosTimeline(int id, EstadosTimelineDTO estadosDTO);
        Task<Resultado<EstadosTimelineDTO>> CrearEstadosTimeline(EstadosTimelineDTO estados);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<List<EstadosTimeline>>> EstadosTimeline();
        Task<Resultado<EstadosTimelineDTO>> EstadosTimelineId(int id);
    }
}