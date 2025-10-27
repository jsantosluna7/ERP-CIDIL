using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
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


        public async Task<Resultado<ReporteFallaDTO?>> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO)
        {
            var reporteAll = await repositorioReporteFalla.ActualizarReporte(id, actualizarReporteFallaDTO);
            var reporte = reporteAll.Valor;
            if (!reporteAll.esExitoso)
            {
                return Resultado<ReporteFallaDTO?>.Falla(reporteAll.MensajeError ?? "No se puede actualizar reporte de falla.");
            }

            var reporteFallaDTO = new ReporteFallaDTO
            {
                IdReporte = id,
                Estado = reporte.Estado,
            };

            return Resultado<ReporteFallaDTO?>.Exito(reporteFallaDTO);
        }

        public async Task<Resultado<List<ReporteFalla>>> ObtenerReporteFallaUsuario(int id)
        {
            var resultado = await repositorioReporteFalla.ObtenerReporteFallaUsuario(id);

            if (!resultado.esExitoso)
            {
                return Resultado<List<ReporteFalla>>.Falla("No haz realizado reportes de falla.");
            }

            return Resultado<List<ReporteFalla>>.Exito(resultado.Valor!);
        }

        public async Task<Resultado<ReporteFallaDTO?>> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO)
        {
            var reporteAll = await repositorioReporteFalla.CrearReporte(crearReporteFallaDTO);
            var reporte = reporteAll.Valor;
            if(!reporteAll.esExitoso)
            {
                return Resultado<ReporteFallaDTO?>.Falla(reporteAll.MensajeError ?? "No se puede crear un reporte de falla.");
            }

            var reporteFallaDTO = new ReporteFallaDTO
            {
                IdReporte = reporte.IdReporte,
                Descripcion = reporte.Descripcion,
                Lugar = reporte.Lugar,
                Estado = reporte.Estado,
                FechaCreacion = DateTime.UtcNow,
                IdUsuario = reporte.IdUsuario,
                FechaUltimaActualizacion = reporte.FechaUltimaActualizacion

            };
            return Resultado<ReporteFallaDTO?>.Exito(reporteFallaDTO);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var reporteAll = await repositorioReporteFalla.Eliminar(id);
            var reporte = reporteAll.Valor;
            if (!reporteAll.esExitoso)
            {
                return Resultado<bool?>.Falla(reporteAll.MensajeError ?? "No se pudo eliminar el reporte de falla.");
            }
            return Resultado<bool?>.Exito(reporte);
        }

        public async Task<Resultado<ReporteFalla?>> GetByIdReporteFalla(int id)
        {
            var reporteAll = await repositorioReporteFalla.GetByIdReporteFalla(id);
            var reporte = reporteAll.Valor;

            if (!reporteAll.esExitoso)
            {
                return Resultado<ReporteFalla?>.Falla(reporteAll.MensajeError ?? "No se pudo encontrar el reporte de falla.");
            }

            return Resultado<ReporteFalla?>.Exito(reporte);
        }

        public async Task<Resultado<List<ReporteFallaDTO?>>> GetReporteFalla()
        {
            var reporteAll = await repositorioReporteFalla.GetReporteFalla();
            var reporte = reporteAll.Valor;

            if (!reporteAll.esExitoso)
            {
                return Resultado<List<ReporteFallaDTO?>>.Falla(reporteAll.MensajeError ?? "No se encontraton reportes de falla.");
            }

            var reporteFallaDTO = new List<ReporteFallaDTO>();

            foreach(ReporteFalla r in reporte)
            {
                var nuevoReporte = new ReporteFallaDTO
                {
                    IdReporte = r.IdReporte,
                    Descripcion = r.Descripcion,
                    Lugar = r.Lugar,
                    FechaCreacion = r.FechaCreacion,
                    FechaUltimaActualizacion =r.FechaUltimaActualizacion,
                    Estado = r.Estado,
                    IdUsuario = r.IdUsuario
                };
                reporteFallaDTO.Add(nuevoReporte);
            }
            return Resultado<List<ReporteFallaDTO?>>.Exito(reporteFallaDTO);
        }
    }
}
