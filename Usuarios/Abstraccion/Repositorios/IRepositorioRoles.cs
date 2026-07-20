using ERP.Data.Modelos;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioRoles
    {
        Task<List<Role>> obtenerRoles();
        Task<Role?> obtenerRolesPorId(int id);
    }
}