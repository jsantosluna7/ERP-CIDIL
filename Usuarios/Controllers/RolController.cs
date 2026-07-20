using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IServicioRoles _servicioRoles;

        public RolController(IServicioRoles servicioRoles)
        {
            _servicioRoles = servicioRoles;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerRoles()
        {
            var roles = await _servicioRoles.obtenerRolesDTO();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerRolPorId(int id)
        {
            var rol = await _servicioRoles.obtenerRolesPorId(id);
            if (rol == null)
            {
                return NotFound($"No se encontró el rol con el id {id}");
            }
            return Ok(rol);
        }
    }
}
