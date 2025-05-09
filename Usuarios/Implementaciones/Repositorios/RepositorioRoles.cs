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

        public List<Roles> obtenerRoles()
        {
            return [.. _contexto.Roles];
        }

        public Roles obtenerRolesPorId(int id)
        {
            return _contexto.Roles.Include(r => r.Usuarios).Where(r => r.Id == id).FirstOrDefault();
        }
    }
}
