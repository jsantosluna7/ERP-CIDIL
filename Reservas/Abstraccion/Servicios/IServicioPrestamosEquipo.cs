using ERP.Data.Modelos;
using Reservas.DTO.DTOPrestamosEquipo;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioPrestamosEquipo
    {
        Task<Resultado<bool>> Eliminar(int id);
        Task<Resultado<PrestamosEquipoDTO>> MarcarDevuelto(int id, MarcarDevueltoDTO dto);
        Task<Resultado<List<ExtensionDTO>>> ObtenerExtensionsPendientes();
        Task<Resultado<List<ExtensionDTO>>> ObtenerExtensionsPorPrestamo(int idPrestamo);
        Task<Resultado<PrestamosEquipoDTO>> ObtenerPorId(int id);
        Task<Resultado<List<PrestamosEquipoDTO>>> ObtenerPrestamos(string? estado = null, int? idUsuario = null, int pagina = 1, int tamanoPagina = 20);
        Task<Resultado<ResumenPrestamosDTO>> ObtenerResumen();
        Task<Resultado<object>> Procesar(ProcesarPrestamosEquipoDTO dto);
        Task<Resultado<ExtensionDTO>> SolicitarExtension(int idPrestamo, CrearExtensionDTO dto);
    }
}
