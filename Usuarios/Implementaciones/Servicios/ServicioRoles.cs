using ERP.Data.Modelos;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.RolDTO;
using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioRoles : IServicioRoles
    {
        private readonly IRepositorioRoles _repositorioRoles;

        public ServicioRoles(IRepositorioRoles repositorioRoles)
        {
            _repositorioRoles = repositorioRoles;
        }

        public async Task<List<rolDTO>> obtenerRolesDTO()
        {
            var roles = await _repositorioRoles.obtenerRoles();
            var rolesDTO = new List<rolDTO>();

            foreach (Role rol in roles)
            {
                var todosLosRoles = new rolDTO
                {
                    Id = rol.Id,
                    Rol = rol.Rol,
                };
                rolesDTO.Add(todosLosRoles);
            }
            return rolesDTO;
        }

        public async Task<Role?> obtenerRolesPorId(int id)
        {
            var rol = await _repositorioRoles.obtenerRolesPorId(id);
            if (rol == null)
            {
                return null;
            }

            return new Role
            {
                Id = rol.Id,
                Rol = rol.Rol,
            };       
        }
    }
}
