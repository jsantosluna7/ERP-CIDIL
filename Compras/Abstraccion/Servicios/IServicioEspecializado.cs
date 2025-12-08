using Compras.DTO.EspecializadosDTO;
using ERP.Data.Modelos;

namespace Compras.Abstraccion.Servicios
{
    public interface IServicioEspecializado
    {
        Task<Resultado<object>> ActualizarEstadoOrden(int ordenId, ActualizarEstadoOrdenDTO actualizarEstadoOrdenDTO);
        Task<Resultado<object>> ActualizarItemRecepcion(int itemId, ActualizarItemRecepcionDTO actualizarItemRecepcionDTO);
        Task<Resultado<List<OrdenItem>>> ObtenerItems(int ordenId);
        Task<Resultado<List<TimelineDTO>>> ObtenerTimeline(int ordenId);
        Task<Resultado<object>> RecalcularEstadoOrden(int ordenId, int usuarioId);
    }
}