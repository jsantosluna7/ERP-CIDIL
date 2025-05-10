using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioRoles
    {
        Task<List<Roles>> obtenerRoles();
        Task<Roles?> obtenerRolesPorId(int id);
    }
}