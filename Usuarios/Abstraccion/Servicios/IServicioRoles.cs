using Usuarios.DTO.RolDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioRoles
    {
        List<rolDTO> obtenerRolesDTO();
        Roles obtenerRolesPorId(int id);
    }
}