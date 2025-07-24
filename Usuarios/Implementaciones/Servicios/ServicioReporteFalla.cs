using ERP.Data.Modelos;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.ReporteFallaDTO;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioReporteFalla : IServicioReporteFalla
    {



        private readonly IRepositorioReporteFalla repositorioReporteFalla;


        public ServicioReporteFalla(IRepositorioReporteFalla _ReporteFalla)
        {
               repositorioReporteFalla = _ReporteFalla;
        }


        public async Task<ReporteFallaDTO?> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO)
        {
           var reporte = await repositorioReporteFalla.ActualizarReporte(id, actualizarReporteFallaDTO);
            if (reporte == null)
            {
                return null;
            }

            var reporteFallaDTO = new ReporteFallaDTO
            {
                IdEstado = id,
                IdReporte =id
            };

            return reporteFallaDTO;
        }

        public async Task<ReporteFallaDTO?> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO)
        {
            var reporte = await repositorioReporteFalla.CrearReporte(crearReporteFallaDTO);
            if(reporte == null)
            {
                return null;
            }

            var reporteFallaDTO = new ReporteFallaDTO
            {
                IdEstado = reporte.IdEstado,
                Descripcion = reporte.Descripcion,
                NombreSolicitante = reporte.NombreSolicitante,
                Lugar = reporte.Lugar,
                IdLaboratorio = reporte.IdLaboratorio,
            };
            return reporteFallaDTO;
        }

        public async Task<bool?> Eliminar(int id)
        {
            var reporte = await repositorioReporteFalla.Eliminar(id);
            if(reporte == null)
            {
                return null;
            }
            return reporte;
        }

        public async Task<ReporteFalla?> GetByIdReporteFalla(int id)
        {
           return await repositorioReporteFalla.GetByIdReporteFalla(id);
        }

        public async Task<List<ReporteFallaDTO?>> GetReporteFalla()
        {
            var reporte = await repositorioReporteFalla.GetReporteFalla();
            if(reporte == null)
            {
                return null;
            }

            var reporteFallaDTO = new List<ReporteFallaDTO>();
            foreach(ReporteFalla r in reporte)
            {
                var nuevoReporte = new ReporteFallaDTO
                {
                    IdReporte = r.IdReporte,
                    IdLaboratorio = r.IdLaboratorio,
                    Descripcion = r.Descripcion,
                    NombreSolicitante = r.NombreSolicitante,
                    Lugar = r.Lugar,
                    FechaCreacion = r.FechaCreacion,
                    FechaUltimaActualizacion =r.FechaUltimaActualizacion,
                    IdEstado = r.IdEstado,
                };
                reporteFallaDTO.Add(nuevoReporte);
            }
            return reporteFallaDTO;
        }
    }
}
