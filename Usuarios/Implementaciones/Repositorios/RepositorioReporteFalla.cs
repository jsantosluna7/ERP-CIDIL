using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.DTO.ReporteFallaDTO;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioReporteFalla : IRepositorioReporteFalla
    {

        private readonly DbErpContext _context;

        public RepositorioReporteFalla(DbErpContext context)
        {
            _context = context;
        }

        public async Task<ReporteFalla?> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO)
        {
            var reporteExiste = await GetByIdReporteFalla(id);
            if (reporteExiste == null)
            {
                return null;
            }

            reporteExiste.IdReporte = actualizarReporteFallaDTO.IdReporte;
            reporteExiste.IdEstado = actualizarReporteFallaDTO.IdEstado;
            reporteExiste.FechaUltimaActualizacion = DateTime.UtcNow;

            _context.Update(reporteExiste);
            await _context.SaveChangesAsync();
            var reporteActualizado = await GetByIdReporteFalla(id);
            return reporteActualizado;
        }

        public async Task<ReporteFalla?> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO)
        {
            var Reporte = new ReporteFalla
            {
                IdLaboratorio = crearReporteFallaDTO.IdLaboratorio,
                Descripcion = crearReporteFallaDTO.Descripcion,
                NombreSolicitante = crearReporteFallaDTO.NombreSolicitante,
                IdEstado = crearReporteFallaDTO.IdEstado,
                Lugar = crearReporteFallaDTO.Lugar,
            };

            _context.ReporteFallas.Add(Reporte);
            await _context.SaveChangesAsync();
            return Reporte;
        }

        public async Task<bool?> Eliminar(int id)
        {
            var reporte = await GetByIdReporteFalla(id);
            if (reporte == null)
            {
                return false;
            }

            _context.Remove(reporte);
            _context.SaveChanges();
            return true;
        }

        public async Task<ReporteFalla?> GetByIdReporteFalla(int id)
        {
            return await _context.ReporteFallas.Where(r => r.IdReporte== id).FirstOrDefaultAsync();

        }

        public async Task<List<ReporteFalla?>> GetReporteFalla()
        {
            return await _context.ReporteFallas.Where(r => r != null).ToListAsync();
        }

    }
}
