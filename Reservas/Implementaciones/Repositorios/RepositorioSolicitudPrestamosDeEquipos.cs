using ERP.Data.Modelos;
using Inventario.DTO.InventarioEquipoDTO;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.DTO.DTOSolicitudDeEquipos;
using Reservas.Implementaciones.Servicios;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioSolicitudPrestamosDeEquipos : IRepositorioSolicitudPrestamosDeEquipos
    {
        private readonly DbErpContext _context;

        public RepositorioSolicitudPrestamosDeEquipos(DbErpContext context)
        {
            _context = context;
        }

        public async Task<List<SolicitudPrestamosDeEquiposDTO>> ObtenerTodas(int pagina, int tamanoPagina)
            => await ProyectarDTO(
                _context.SolicitudPrestamosDeEquipos
                    .OrderByDescending(s => s.FechaSolicitud)
                    .Skip((pagina - 1) * tamanoPagina)
                    .Take(tamanoPagina)
            ).ToListAsync();

        public async Task<SolicitudPrestamosDeEquiposDTO?> ObtenerPorIdTodo(int id)
            => await ProyectarDTO(
                _context.SolicitudPrestamosDeEquipos
                    .Where(s => s.Id == id)
            ).FirstOrDefaultAsync();

        public async Task<SolicitudPrestamosDeEquipo?> ObtenerPorId(int id)
    => await _context.SolicitudPrestamosDeEquipos
            .Where(s => s.Id == id).FirstOrDefaultAsync();

        public async Task<List<SolicitudPrestamosDeEquiposDTO>> ObtenerPorUsuario(int idUsuario)
            => await ProyectarDTO(
                _context.SolicitudPrestamosDeEquipos
                    .Where(s => s.IdUsuario == idUsuario)
                    .OrderByDescending(s => s.FechaSolicitud)
            ).ToListAsync();

        /// <summary>
        /// Suma la cantidad total ya reservada de un equipo en un rango de fechas.
        /// Esto permite validar si queda suficiente stock disponible.
        /// </summary>
        public async Task<int> ObtenerCantidadReservada(int idInventario, DateTime fechaInicio, DateTime fechaFinal, int? excludeId = null)
            => await _context.PrestamosEquipos
                .Where(s => s.IdInventario == idInventario)
                .Where(s => excludeId == null || s.Id != excludeId)
                .Where(s => s.IdEstado == 1)
                .Where(s => s.FechaInicio < fechaFinal && s.FechaFinal > fechaInicio)
                .SumAsync(s => s.Cantidad ?? 0);

       public async Task<int> ObtenerCantidadReservadaEnRango(int idInventario, DateTime fechaInicio, DateTime fechaFinal, List<int> estados, int? excludeId = null)
            => await _context.SolicitudPrestamosDeEquipos
                .Where(s => s.IdInventario == idInventario)
                .Where(s => excludeId == null || s.Id != excludeId)
                .Where(s => estados.Contains(s.IdEstado ?? 0))
                .Where(s => s.FechaInicio < fechaFinal && s.FechaFinal > fechaInicio)
                .SumAsync(s => s.Cantidad ?? 0);

        public async Task<InventarioEquipo?> ObtenerInventarioPorId(int id)
            => await _context.InventarioEquipos.FindAsync(id);

        public async Task<List<Usuario>> ObtenerAdmins()
            => await _context.Usuarios
                .Where(u => u.IdRol == 1 || u.IdRol == 2)
                .ToListAsync();

        public async Task<SolicitudPrestamosDeEquipo> Crear(SolicitudPrestamosDeEquipo solicitud)
        {
            _context.SolicitudPrestamosDeEquipos.Add(solicitud);
            return solicitud;
        }

        public async Task<SolicitudPrestamosDeEquipo> Actualizar(SolicitudPrestamosDeEquipo solicitud)
        {
            _context.SolicitudPrestamosDeEquipos.Update(solicitud);
            return solicitud;
        }

        public async Task Eliminar(int idSolicitud) {
            var solicitud = await _context.SolicitudPrestamosDeEquipos.FindAsync(idSolicitud);

            if (solicitud != null) {
                _context.SolicitudPrestamosDeEquipos.Remove(solicitud);
            }
}

        public async Task GuardarCambios()
            => await _context.SaveChangesAsync();

        private static IQueryable<SolicitudPrestamosDeEquiposDTO> ProyectarDTO(IQueryable<SolicitudPrestamosDeEquipo> query)
    => query.Select(s => new SolicitudPrestamosDeEquiposDTO
    {
        Id = s.Id,
        IdUsuario = s.IdUsuario,
        IdInventario = s.IdInventario,
        Inventario = new InventarioEquipoDTO  // EF hace el JOIN automáticamente
        {
            Id = s.IdInventarioNavigation.Id,
            Nombre = s.IdInventarioNavigation.Nombre,
            NombreCorto = s.IdInventarioNavigation.NombreCorto,
            Perfil = s.IdInventarioNavigation.Perfil,
            IdLaboratorio = s.IdInventarioNavigation.IdLaboratorio,
            Fabricante = s.IdInventarioNavigation.Fabricante,
            Modelo = s.IdInventarioNavigation.Modelo,
            Serial = s.IdInventarioNavigation.Serial,
            DescripcionLarga = s.IdInventarioNavigation.DescripcionLarga,
            FechaTransaccion = s.IdInventarioNavigation.FechaTransaccion,
            Departamento = s.IdInventarioNavigation.Departamento,
            ImporteActivo = s.IdInventarioNavigation.ImporteActivo,
            ImagenEquipo = s.IdInventarioNavigation.ImagenEquipo,
            Disponible = s.IdInventarioNavigation.Disponible,
            IdEstadoFisico = s.IdInventarioNavigation.IdEstadoFisico,
            ValidacionPrestamo = s.IdInventarioNavigation.ValidacionPrestamo,
            Cantidad = s.IdInventarioNavigation.Cantidad,
            Activado = s.IdInventarioNavigation.Activado,
        },
        FechaInicio = s.FechaInicio,
        FechaFinal = s.FechaFinal,
        Motivo = s.Motivo,
        FechaSolicitud = s.FechaSolicitud,
        IdEstado = s.IdEstado,
        Cantidad = s.Cantidad,
    });
    }
}
