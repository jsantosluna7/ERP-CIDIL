using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioSolicitudDeReserva
    {
        Task<bool?> CancelarSolicitudReserva(int id);
        Task<ActualizarSolicitudDeReservaDTO?> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO);
        Task<List<SolicitudDeReservaDTO>?> ObtenerSolicitudesReservas(int pagina, int tamanoPagina);
        Task<List<SolicitudDeReservaDTO>?> ObtenerSolicitudesReservasPorPiso(int piso);
        Task<SolicitudDeReservaDTO?> ObtenerSolicitudReservaPorId(int id);
        Task<CrearSolicitudDeReservaDTO?> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO);
    }
}