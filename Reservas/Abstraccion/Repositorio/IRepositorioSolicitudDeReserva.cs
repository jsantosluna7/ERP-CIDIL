using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioSolicitudDeReserva
    {
        Task<bool?> CancelarSolicitudReserva(int id);
        Task<SolicitudReservaDeEspacio?> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO);
        Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservas(int pagina, int tamanoPagina);
        Task<SolicitudReservaDeEspacio?> ObtenerSolicitudReservaPorId(int id);
        Task<SolicitudReservaDeEspacio?> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO);
    }
}