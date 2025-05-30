using Reservas.DTO.DTOSolicitudDeEquipos;
using Reservas.Modelos;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioSolicitudPrestamosDeEquipos
    {
        Task<SolicitudPrestamosDeEquipo?> ActualizarSolicitudPEquipos(int id, ActualizarSolicitudPrestamosDeEquiposDTO actualizarSolicitudPrestamosDeEquiposDTO);
        Task<bool?> CancelarSolicitudReserva(int id);
        Task<bool> conflictoPrestamos(int IdUsuario, int IdInventario, DateTime? FechaInicio, DateTime? FechaFinal, DateTime? FechaSolicitud);
        Task<bool> conflictoReservaActualizar(int IdUsuario, int IdInventario, DateTime? FechaInicio, DateTime? FechaFinal, DateTime? FechaSolicitud);
        Task<SolicitudPrestamosDeEquipo?> CrearSolicitudPEquipos(CrearSolicitudPrestamosDeEquiposDTO crearSolicitudPrestamosDeEquiposDTO);
        Task<SolicitudPrestamosDeEquipo> GetByIdSolicitudPEquipos(int id);
        Task<List<SolicitudPrestamosDeEquipo>> GetSolicitudPrestamos(int pagina, int tamanoPagina);
    }
}