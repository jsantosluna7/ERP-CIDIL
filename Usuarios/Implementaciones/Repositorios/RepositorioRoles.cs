using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioRoles: IRepositorioRoles
    {
        private readonly DbErpContext _contexto;

        public RepositorioRoles(DbErpContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Role>> obtenerRoles()
        {
            return await _contexto.Roles.ToListAsync();
        }

        public async Task<Role?> obtenerRolesPorId(int id)
        {
            return await _contexto.Roles
                .Include(r => r.Usuarios)
                    .ThenInclude(u => u.PrestamosEquipoIdUsuarioAprobadorNavigations)
                .Include(r => r.Usuarios)
                    .ThenInclude(u => u.PrestamosEquipoIdUsuarioNavigations)
                .Include(r => r.Usuarios)
                    .ThenInclude(u => u.ReservaDeEspacioIdUsuarioAprobadorNavigations)
                .Include(r => r.Usuarios)
                    .ThenInclude(u => u.ReservaDeEspacioIdUsuarioNavigations)
                .Include(r => r.Usuarios)
                    .ThenInclude(u => u.SolicitudPrestamosDeEquipos)
                .Include(r => r.Usuarios)
                    .ThenInclude(u => u.SolicitudReservaDeEspacios)
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }
    }
}
