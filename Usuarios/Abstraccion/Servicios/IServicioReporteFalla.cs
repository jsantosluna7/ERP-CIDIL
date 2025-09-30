using ERP.Data.Modelos;
using Usuarios.DTO.ReporteFallaDTO;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioReporteFalla
    {
        Task<Resultado<ReporteFallaDTO?>> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO);
        Task<Resultado<ReporteFallaDTO?>> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<ReporteFalla?>> GetByIdReporteFalla(int id);
        Task<Resultado<List<ReporteFallaDTO?>>> GetReporteFalla();
    }
}
