using ERP.Data.Modelos;
using Reservas.DTO.DTOTotalReservaEspacios;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioTotalReservaDeEspacio
    {
        Task<int> ContarSolicitudesAprobadas();
        Task<int> ContarSolicitudesPendientes();
        Task<int> ContarSolicitudesRechazadas();
        Task<int> ContarTotalSolicitudes();
        Task<List<Estado>> ObtenerEstados();
        Task<List<Laboratorio>> ObtenerLaboratoriosPorIds(List<int> ids);
        Task<List<ReservaDeEspacio>> ObtenerReservasResueltas();
        Task<List<ReservaDeEspacioUsuarioDTO>> ObtenerReservasResueltasPorUsuario(int idUsuario);
        Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesPendientes();
        Task<List<ReservaDeEspacioUsuarioDTO>> ObtenerSolicitudesPendientesPorUsuario(int idUsuario);
        Task<List<Usuario>> ObtenerUsuariosPorIds(List<int> ids);
    }
}