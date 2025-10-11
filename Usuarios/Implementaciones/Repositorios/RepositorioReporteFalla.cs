using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.DTO.ReporteFallaDTO;
using Usuarios.Implementaciones.Servicios;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioReporteFalla : IRepositorioReporteFalla
    {

        private readonly DbErpContext _context;
        private readonly ServicioEmailUsuarios _servicioEmail;

        public RepositorioReporteFalla(DbErpContext context, ServicioEmailUsuarios servicioEmail)
        {
            _context = context;
            _servicioEmail = servicioEmail;
        }

        public async Task<Resultado<ReporteFalla?>> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO)
        {
            var reporteId = await GetByIdReporteFalla(id);
            var reporteExiste = reporteId.Valor;

            if (reporteExiste == null)
            {
                return Resultado<ReporteFalla?>.Falla("No se encontró un reporte de falla con ese id.");
            }

            reporteExiste.IdReporte = actualizarReporteFallaDTO.IdReporte;
            reporteExiste.Estado = actualizarReporteFallaDTO.Estado;
            reporteExiste.FechaUltimaActualizacion = DateTime.UtcNow;

            var usuario = await _context.Usuarios.Where(u => u.Id == reporteExiste.IdUsuario).FirstOrDefaultAsync();

            if (reporteExiste.Estado == 2)
            {
                await _servicioEmail.EnviarCorreoRecepcionReporte(usuario.CorreoInstitucional, reporteExiste.Descripcion, reporteExiste.Lugar, $"{usuario.NombreUsuario} {usuario.ApellidoUsuario}"); //Correo para confirmar el reporte de falla al usuario que lo reporto.
            }

            if (reporteExiste.Estado == 3)
            {
                await _servicioEmail.EnviarCorreoSolucionReporte(usuario.CorreoInstitucional, reporteExiste.Descripcion, reporteExiste.Lugar, $"{usuario.NombreUsuario} {usuario.ApellidoUsuario}"); //Correo para avisar solucion al reporte de falla al usuario que lo reporto.
            }

                _context.Update(reporteExiste);
            await _context.SaveChangesAsync();
            var reporte = await GetByIdReporteFalla(id);
            var reporteActualizado = reporte.Valor;

            return Resultado<ReporteFalla?>.Exito(reporteActualizado);
        }

        public async Task<Resultado<ReporteFalla?>> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO)
        {
            try
            {
                var Reporte = new ReporteFalla
                {
                    Descripcion = crearReporteFallaDTO.Descripcion,
                    Lugar = crearReporteFallaDTO.Lugar,
                    Estado = crearReporteFallaDTO.Estado,
                    IdUsuario = crearReporteFallaDTO.IdUsuario
                };

                var roles = new int?[] { 1,2 }; //Roles de administrador y superusuario.

                var usuario = await _context.Usuarios.Where(u => roles.Contains(u.IdRol)).ToListAsync();
                var usuarioReporta = await _context.Usuarios.Where(u => u.Id == crearReporteFallaDTO.IdUsuario).FirstOrDefaultAsync();

                foreach (var usuarios in usuario)
                {
                    await _servicioEmail.EnviarCorreoNuevoReporte(usuarios.CorreoInstitucional, Reporte.Descripcion, Reporte.Lugar, $"{usuarioReporta.NombreUsuario} {usuarioReporta.ApellidoUsuario}"); //Correo para que se le envie el reporte de falla a todos los administradores.
                }

                _context.ReporteFallas.Add(Reporte);
                await _context.SaveChangesAsync();
                return Resultado<ReporteFalla?>.Exito(Reporte);
            }
            catch (Exception ex)
            {
                return Resultado<ReporteFalla?>.Falla($"Error al crear el reporte: {ex.Message}");
            }
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var reporte = await GetByIdReporteFalla(id);
            if (reporte == null)
            {
                return Resultado<bool?>.Falla("No se puede eliminar el reporte porque no existe.");
            }

            _context.Remove(reporte);
            _context.SaveChanges();
            return Resultado<bool?>.Exito(true);
        }

        public async Task<Resultado<ReporteFalla?>> GetByIdReporteFalla(int id)
        {
            var resultado = await _context.ReporteFallas.Where(r => r.IdReporte== id).FirstOrDefaultAsync();
            if(resultado == null)
            {
                return Resultado<ReporteFalla?>.Falla("No se encontró un reporte de falla con ese id.");
            }

            return Resultado<ReporteFalla?>.Exito(resultado);
        }

        public async Task<Resultado<List<ReporteFalla?>>> GetReporteFalla()
        {
            var resultado = await _context.ReporteFallas.Where(r => r != null).ToListAsync();
            if(resultado == null)
            {
                return Resultado<List<ReporteFalla?>>.Falla("No se encontraron reportes de falla.");
            }
            return Resultado<List<ReporteFalla?>>.Exito(resultado);
        }

    }
}
