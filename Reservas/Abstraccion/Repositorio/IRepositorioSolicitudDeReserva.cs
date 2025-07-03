using ERP.Data.Modelos;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioSolicitudDeReserva
    {
        Task<bool?> CancelarSolicitudReserva(int id);
        Task<SolicitudReservaDeEspacio?> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO);
        Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservas(int pagina, int tamanoPagina);
        Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservasPorPiso(int piso);
        Task<SolicitudReservaDeEspacio?> ObtenerSolicitudReservaPorId(int id);
        Task<SolicitudReservaDeEspacio?> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO);
    }
}