using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioReservaDeEspacio
    {
        Task<bool?> CancelarReserva(int id);
        Task<bool?> CancelarSolicitudReserva(int id);
        Task<CrearReservaDeEspacioDTO?> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO);
        Task<ActualizarReservaDeEspacioDTO?> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO);
        Task<ActualizarSolicitudDeReservaDTO?> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO);
        Task<ReservaDeEspacioDTO?> ObtenerReservaPorId(int id);
        Task<List<ReservaDeEspacioDTO>?> ObtenerReservas();
        Task<List<SolicitudDeReservaDTO>?> ObtenerSolicitudesReservas();
        Task<SolicitudDeReservaDTO?> ObtenerSolicitudReservaPorId(int id);
        Task<CrearSolicitudDeReservaDTO?> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO);
    }
}