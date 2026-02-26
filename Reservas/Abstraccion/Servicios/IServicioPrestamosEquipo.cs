using ERP.Data.Modelos;
using Reservas.DTO.DTOPrestamosEquipo;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioPrestamosEquipo
    {
        Task<Resultado<ExtensionDTO>> AprobarRechazarExtension(int idExtension, AprobarRechazarExtensionDTO dto);
        Task<Resultado<bool>> Eliminar(int id);
        Task<Resultado<PrestamosEquipoDTO>> MarcarDevuelto(int id, MarcarDevueltoDTO dto);
        Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerActivos();
        Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerAtrasados();
        Task<Resultado<List<ExtensionDTO>>> ObtenerExtensionsPendientes();
        Task<Resultado<List<ExtensionDTO>>> ObtenerExtensionsPorPrestamo(int idPrestamo);
        Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerPendientes();
        Task<Resultado<PrestamosEquipoDTO>> ObtenerPorId(int id);
        Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerPorUsuario(int idUsuario);
        Task<Resultado<ResumenPrestamosDTO>> ObtenerResumen();
        Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerTodos(int pagina, int tamanoPagina);
        Task<Resultado<PrestamosEquipoDTO>> ProcesarSolicitud(AprobarRechazarSolicitudDTO dto);
        Task<Resultado<ExtensionDTO>> SolicitarExtension(int idPrestamo, CrearExtensionDTO dto);
    }
}
