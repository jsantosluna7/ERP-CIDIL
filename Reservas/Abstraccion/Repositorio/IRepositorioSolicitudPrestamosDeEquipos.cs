using ERP.Data.Modelos;
using Reservas.DTO.DTOSolicitudDeEquipos;

namespace Reservas.Abstraccion.Repositorio
{
    public interface IRepositorioSolicitudPrestamosDeEquipos
    {
        Task<SolicitudPrestamosDeEquipo> Actualizar(SolicitudPrestamosDeEquipo solicitud);
        Task<SolicitudPrestamosDeEquipo> Crear(SolicitudPrestamosDeEquipo solicitud);
        Task Eliminar(int idSolicitud);
        Task GuardarCambios();
        Task<List<Usuario>> ObtenerAdmins();
        Task<int> ObtenerCantidadReservada(int idInventario, DateTime fechaInicio, DateTime fechaFinal, int? excludeId = null);
        Task<int> ObtenerCantidadReservadaEnRango(int idInventario, DateTime fechaInicio, DateTime fechaFinal, List<int> estados, int? excludeId = null);
        Task<InventarioEquipo?> ObtenerInventarioPorId(int id);
        Task<SolicitudPrestamosDeEquipo?> ObtenerPorId(int id);
        Task<SolicitudPrestamosDeEquiposDTO?> ObtenerPorIdTodo(int id);
        Task<List<SolicitudPrestamosDeEquiposDTO>> ObtenerPorUsuario(int idUsuario);
        Task<List<SolicitudPrestamosDeEquiposDTO>> ObtenerTodas(int pagina, int tamanoPagina);
    }
}