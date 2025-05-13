using Usuarios.DTO.RolDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioRoles
    {
        Task<List<rolDTO>> obtenerRolesDTO();
        Task<Role?> obtenerRolesPorId(int id);
    }
}