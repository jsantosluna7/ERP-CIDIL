using ERP.Data.Modelos;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioSolicitudDeReserva
    {
        Task<Resultado<bool?>> CancelarSolicitudReserva(int id);
        Task<Resultado<SolicitudReservaDeEspacio?>> EditarSolicitudReserva(int id, ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO);
        Task<Resultado<List<SolicitudReservaDeEspacio>>> ObtenerSolicitudEspaciosUsuario(int id);
        Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservas(int pagina, int tamanoPagina);
        Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesReservasPorPiso(int piso);
        Task<Resultado<SolicitudReservaDeEspacio?>> ObtenerSolicitudReservaPorId(int id);
        Task<Resultado<SolicitudReservaDeEspacio?>> SolicitarCrearReserva(CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO);
    }
}