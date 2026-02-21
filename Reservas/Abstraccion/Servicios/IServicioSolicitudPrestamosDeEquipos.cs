using ERP.Data.Modelos;
using Reservas.DTO.DTOSolicitudDeEquipos;

namespace Reservas.Abstraccion.Servicios
{
    public interface IServicioSolicitudPrestamosDeEquipos
    {
        Task<Resultado<SolicitudPrestamosDeEquiposDTO>> Actualizar(int id, ActualizarSolicitudPrestamosDeEquiposDTO dto);
        Task<ResultadoCrearMultiplesDTO> CrearMultiples(CrearSolicitudPrestamosDeEquiposDTO dto);
        Task<Resultado<bool>> Eliminar(int id);
        Task<Resultado<SolicitudPrestamosDeEquiposDTO>> ObtenerPorId(int id);
        Task<Resultado<List<SolicitudPrestamosDeEquiposDTO>>> ObtenerPorUsuario(int idUsuario);
        Task<Resultado<List<SolicitudPrestamosDeEquiposDTO>>> ObtenerTodas(int pagina, int tamanoPagina);
    }
}