using ERP.Data.Modelos;
using Usuarios.DTO.ReporteFallaDTO;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioReporteFalla
    {
        Task<List<ReporteFallaDTO?>> GetReporteFalla();
        Task<ReporteFalla?> GetByIdReporteFalla(int id);
        Task<ReporteFallaDTO?> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO);
        Task<ReporteFallaDTO?> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO);
        Task<bool?> Eliminar (int  id);




    }
}
