using ERP.Data.Modelos;
using Usuarios.DTO.ReporteFallaDTO;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioReporteFalla
    {
        Task<Resultado<ReporteFalla?>> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO);
        Task<Resultado<ReporteFalla?>> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO);
        Task<Resultado<bool?>> Eliminar(int id);
        Task<Resultado<ReporteFalla?>> GetByIdReporteFalla(int id);
        Task<Resultado<List<ReporteFalla?>>> GetReporteFalla();
        Task<Resultado<List<ReporteFalla>>> ObtenerReporteFallaUsuario(int id);
    }
}
