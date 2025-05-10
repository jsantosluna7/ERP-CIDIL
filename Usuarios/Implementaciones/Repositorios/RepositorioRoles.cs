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

        public async Task<List<Roles>> obtenerRoles()
        {
            return await _contexto.Roles.ToListAsync();
        }

        public async Task<Roles?> obtenerRolesPorId(int id)
        {
            return await _contexto.Roles.Include(r => r.Usuarios).Where(r => r.Id == id).FirstOrDefaultAsync();
        }
    }
}
