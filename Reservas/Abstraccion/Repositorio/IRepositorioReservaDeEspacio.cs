using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioReservaDeEspacio
    {
        Task<bool?> CancelarReserva(int id);
        Task<bool?> CancelarSolicitudReserva(int id);
        Task<ReservaDeEspacio?> CrearReserva(CrearReservaDeEspacioDTO crearReservaDeEspacioDTO);
        Task<ReservaDeEspacio?> EditarReserva(int id, ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO);
        Task<SolicitudReservaDeEspacio?> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO);
        Task<ReservaDeEspacio?> ObtenerReservaPorId(int id);
        Task<List<ReservaDeEspacio>> ObtenerReservas();
        Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservas();
        Task<SolicitudReservaDeEspacio?> ObtenerSolicitudReservaPorId(int id);
        Task<SolicitudReservaDeEspacio?> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO);
    }
}