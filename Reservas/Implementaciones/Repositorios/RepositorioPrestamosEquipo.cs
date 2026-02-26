using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Repositorio;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.Implementaciones.Servicios;

namespace Reservas.Implementaciones.Repositorios
{
    public class RepositorioPrestamosEquipo : IRepositorioPrestamosEquipo
    {
        private readonly DbErpContext _context;

        public RepositorioPrestamosEquipo(DbErpContext context)
        {
            _context = context;
        }


        public async Task<List<PrestamosEquipoDTO>> ObtenerTodos(int pagina, int tamanoPagina)
            => await ProyectarDTO(
                _context.PrestamosEquipos
                    .Where(p => p.Activado == true)
                    .OrderByDescending(p => p.FechaInicio)
                    .Skip((pagina - 1) * tamanoPagina)
                    .Take(tamanoPagina)
            ).ToListAsync();

        public async Task<PrestamosEquipo?> ObtenerPorId(int id)
            => await _context.PrestamosEquipos.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<PrestamosEquipoDTO>> ObtenerPorUsuario(int idUsuario)
            => await ProyectarDTO(
                _context.PrestamosEquipos
                    .Where(p => p.IdUsuario == idUsuario && p.Activado == true)
                    .OrderByDescending(p => p.FechaInicio)
            ).ToListAsync();

        public async Task<List<PrestamosEquipoDTO>> ObtenerPendientes()
            => await ProyectarDTO(
                _context.PrestamosEquipos
                    .Where(p => p.IdEstado == 2 && p.Activado == true)
                    .OrderBy(p => p.FechaInicio)
            ).ToListAsync();

        public async Task<List<PrestamosEquipoDTO>> ObtenerActivos()
            => await ProyectarDTO(
                _context.PrestamosEquipos
                    .Where(p => p.IdEstado == 1
                                && p.FechaInicio <= DateTime.Now
                                && p.FechaFinal >= DateTime.Now
                                && p.FechaEntrega == null
                                && p.Activado == true)
                    .OrderBy(p => p.FechaFinal)
            ).ToListAsync();

        public async Task<List<PrestamosEquipoDTO>> ObtenerAtrasados()
            => await ProyectarDTO(
                _context.PrestamosEquipos
                    .Where(p => p.IdEstado == 1
                                && p.FechaFinal < DateTime.Now
                                && p.FechaEntrega == null
                                && p.Activado == true)
                    .OrderBy(p => p.FechaFinal)
            ).ToListAsync();

        public async Task<int> ContarTodos()
            => await _context.PrestamosEquipos.Where(p => p.Activado == true).CountAsync();

        public async Task<PrestamosEquipo> Crear(PrestamosEquipo prestamo)
        {
            _context.PrestamosEquipos.Add(prestamo);
            return prestamo;
        }

        public async Task<PrestamosEquipo> Actualizar(PrestamosEquipo prestamo)
        {
            _context.PrestamosEquipos.Update(prestamo);
            return prestamo;
        }

        public async Task Eliminar(PrestamosEquipo prestamo)
            => _context.PrestamosEquipos.Remove(prestamo);

        public async Task GuardarCambios()
            => await _context.SaveChangesAsync();


        public async Task<List<ExtensionPrestamosEquipo>> ObtenerExtensionsPendientes()
            => await _context.ExtensionPrestamosEquipos
                .Where(e => e.IdEstado == 2)
                .OrderBy(e => e.FechaSolicitud)
                .ToListAsync();

        public async Task<ExtensionPrestamosEquipo?> ObtenerExtensionPorId(int id)
            => await _context.ExtensionPrestamosEquipos.FirstOrDefaultAsync(e => e.Id == id);

        public async Task<List<ExtensionPrestamosEquipo>> ObtenerExtensionsPorPrestamo(int idPrestamo)
            => await _context.ExtensionPrestamosEquipos
                .Where(e => e.IdPrestamos == idPrestamo)
                .OrderByDescending(e => e.FechaSolicitud)
                .ToListAsync();

        public async Task<ExtensionPrestamosEquipo> CrearExtension(ExtensionPrestamosEquipo extension)
        {
            _context.ExtensionPrestamosEquipos.Add(extension);
            return extension;
        }

        public async Task<ExtensionPrestamosEquipo> ActualizarExtension(ExtensionPrestamosEquipo extension)
        {
            _context.ExtensionPrestamosEquipos.Update(extension);
            return extension;
        }

        public async Task<Usuario?> ObtenerUsuarioPorId(int id)
            => await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<List<Usuario>> ObtenerAdmins()
            => await _context.Usuarios.Where(u => u.IdRol == 1 || u.IdRol == 2).ToListAsync();

        public async Task<InventarioEquipo?> ObtenerInventarioPorId(int id)
            => await _context.InventarioEquipos.FindAsync(id);


        private static IQueryable<PrestamosEquipoDTO> ProyectarDTO(IQueryable<PrestamosEquipo> query)
            => query.Select(p => new PrestamosEquipoDTO
            {
                Id = p.Id,
                IdUsuario = p.IdUsuario,
                IdInventario = p.IdInventario,
                NombreEquipo = p.IdInventarioNavigation.Nombre,
                IdEstado = p.IdEstado,
                NombreEstado = p.IdEstadoNavigation.Estado1,
                FechaInicio = p.FechaInicio,
                FechaFinal = p.FechaFinal,
                FechaEntrega = p.FechaEntrega,
                IdUsuarioAprobador = p.IdUsuarioAprobador,
                Motivo = p.Motivo,
                ComentarioAprobacion = p.ComentarioAprobacion,
                Activado = p.Activado,
                Cantidad = p.Cantidad,
                // EstaAtrasado y DiasAtraso se calculan en el servicio
                EstaAtrasado = false,
                DiasAtraso = null
            });
    }
}
