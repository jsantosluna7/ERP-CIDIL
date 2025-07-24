using ERP.Data.Modelos;
using Usuarios.DTO.ReporteFallaDTO;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioReporteFalla
    {
        Task<List<ReporteFalla?>> GetReporteFalla();
        Task <ReporteFalla?> GetByIdReporteFalla(int id);
        Task <ReporteFalla?> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO);
        Task <ReporteFalla?> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO);
        Task<bool?> Eliminar (int id);

    }
}
