using ERP.Data.Modelos;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioSolicitudDeReserva
    {
        Task<Resultado<bool?>> CancelarSolicitudReserva(int id);
        Task<Resultado<ActualizarSolicitudDeReservaDTO?>> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO);
        Task<Resultado<List<SolicitudReservaDeEspacio>>> ObtenerSolicitudEspaciosUsuario(int id);
        Task<List<SolicitudDeReservaDTO>?> ObtenerSolicitudesReservas(int pagina, int tamanoPagina);
        Task<List<SolicitudDeReservaDTO>?> ObtenerSolicitudesReservasPorPiso(int piso);
        Task<Resultado<SolicitudDeReservaDTO?>> ObtenerSolicitudReservaPorId(int id);
        Task<Resultado<CrearSolicitudDeReservaDTO?>> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO);
    }
}