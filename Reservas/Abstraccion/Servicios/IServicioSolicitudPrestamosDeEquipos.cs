using ERP.Data.Modelos;
using Reservas.DTO.DTOSolicitudDeEquipos;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioSolicitudPrestamosDeEquipos
    {
        Task<ActualizarSolicitudPrestamosDeEquiposDTO?> ActualizarSolicitudPEquipos(int id, ActualizarSolicitudPrestamosDeEquiposDTO actualizarSolicitudPrestamosDeEquiposDTO);
        Task<bool?> CancelarSolicitudReserva(int id);
        Task<CrearSolicitudPrestamosDeEquiposDTO?> CrearSolicitudPEquipos(CrearSolicitudPrestamosDeEquiposDTO crearSolicitudPrestamosDeEquiposDTO);
        Task<SolicitudPrestamosDeEquiposDTO?> GetByIdSolicitudPEquipos(int id);
        Task<List<SolicitudPrestamosDeEquiposDTO>?> GetSolicitudPrestamos(int pagina, int tamanoPagina);
        Task<Resultado<List<SolicitudPrestamosDeEquipo>>> ObtenerSolicitudEquiposUsuario(int id);
    }
}