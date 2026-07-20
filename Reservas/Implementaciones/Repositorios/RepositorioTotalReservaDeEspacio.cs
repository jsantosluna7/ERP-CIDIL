using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOTotalReservaEspacios;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioTotalReservaDeEspacio : IRepositorioTotalReservaDeEspacio
    {
        private readonly DbErpContext _context;

        public RepositorioTotalReservaDeEspacio(DbErpContext context)
        {
            _context = context;
        }

        public async Task<List<ReservaDeEspacioUsuarioDTO>> ObtenerSolicitudesPendientesPorUsuario(int idUsuario)
        {
            return await _context.SolicitudReservaDeEspacios
                .Where(s => s.IdUsuario == idUsuario)
                .Select(s => new ReservaDeEspacioUsuarioDTO
                {
                    Id = s.Id,
                    IdUsuario = s.IdUsuario,
                    IdLaboratorio = s.IdLaboratorio,
                    Motivo = s.Motivo,
                    FechaSolicitud = s.FechaSolicitud,
                    IdEstado = s.IdEstado,
                    NombreEstado = s.IdEstadoNavigation != null ? s.IdEstadoNavigation.Estado1 : "PENDIENTE",
                    TipoRegistro = "Solicitud",
                    HoraInicio = s.HoraInicio,
                    HoraFinal = s.HoraFinal,
                    FechaInicio = s.FechaInicio,
                    FechaFinal = s.FechaFinal,
                    PersonasCantidad = s.PersonasCantidad
                })
                .ToListAsync();
        }

        public async Task<List<ReservaDeEspacioUsuarioDTO>> ObtenerReservasResueltasPorUsuario(int idUsuario)
        {
            return await _context.ReservaDeEspacios
                .Where(r => r.IdUsuario == idUsuario && r.Activado == true)
                .Select(r => new ReservaDeEspacioUsuarioDTO
                {
                    Id = r.Id,
                    IdUsuario = r.IdUsuario,
                    IdLaboratorio = r.IdLaboratorio,
                    Motivo = r.Motivo,
                    FechaSolicitud = r.FechaSolicitud,
                    IdEstado = r.IdEstado,
                    NombreEstado = r.IdEstadoNavigation != null ? r.IdEstadoNavigation.Estado1 : "DESCONOCIDO",
                    TipoRegistro = "Reserva",
                    HoraInicio = r.HoraInicio,
                    HoraFinal = r.HoraFinal,
                    FechaInicio = r.FechaInicio ?? default,
                    FechaFinal = r.FechaFinal ?? default,
                    PersonasCantidad = r.PersonasCantidad,
                    IdUsuarioAprobador = r.IdUsuarioAprobador,
                    FechaAprobacion = r.FechaAprobacion,
                    ComentarioAprobacion = r.ComentarioAprobacion
                })
                .ToListAsync();
        }

        public async Task<List<SolicitudReservaDeEspacio>> ObtenerSolicitudesPendientes()
        {
            return await _context.SolicitudReservaDeEspacios
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<List<ReservaDeEspacio>> ObtenerReservasResueltas()
        {
            return await _context.ReservaDeEspacios
                .Where(r => r.Activado == true)
                .OrderByDescending(r => r.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<List<Usuario>> ObtenerUsuariosPorIds(List<int> ids)
        {
            return await _context.Usuarios
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();
        }

        public async Task<List<Laboratorio>> ObtenerLaboratoriosPorIds(List<int> ids)
        {
            return await _context.Laboratorios
                .Where(l => ids.Contains(l.Id))
                .ToListAsync();
        }

        public async Task<List<Estado>> ObtenerEstados()
        {
            return await _context.Estados.ToListAsync();
        }
        // Total de ambas tablas combinadas
        public async Task<int> ContarTotalSolicitudes()
        {
            var totalSolicitudes = await _context.SolicitudReservaDeEspacios.CountAsync();
            var totalReservas = await _context.ReservaDeEspacios.CountAsync(r => r.Activado == true);
            return totalSolicitudes + totalReservas;
        }

        // Pendientes viven en SolicitudReservaDeEspacio con IdEstado = 2
        public async Task<int> ContarSolicitudesPendientes()
        {
            return await _context.SolicitudReservaDeEspacios
                .CountAsync(s => s.IdEstado == 2);
        }

        // Aprobadas viven en ReservaDeEspacio con IdEstado = 1
        public async Task<int> ContarSolicitudesAprobadas()
        {
            var hoy = DateTime.UtcNow;
            return await _context.ReservaDeEspacios
                .CountAsync(r => r.Activado == true
                              && r.IdEstado == 1
                              && r.FechaAprobacion.HasValue
                              && r.FechaAprobacion.Value.Month == hoy.Month
                              && r.FechaAprobacion.Value.Year == hoy.Year);

        }

        // Rechazadas viven en ReservaDeEspacio con IdEstado = 3
        public async Task<int> ContarSolicitudesRechazadas()
        {
            var hoy = DateTime.UtcNow;
            return await _context.ReservaDeEspacios
                .CountAsync(r => r.Activado == true
                              && r.IdEstado == 3
                              && r.FechaAprobacion.HasValue
                              && r.FechaAprobacion.Value.Month == hoy.Month
                              && r.FechaAprobacion.Value.Year == hoy.Year);
        }
    }
}
