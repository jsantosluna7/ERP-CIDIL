using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioRoles
    {
        List<Roles> obtenerRoles();
        Roles obtenerRolesPorId(int id);
    }
}