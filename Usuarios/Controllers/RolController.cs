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
        public IActionResult ObtenerRoles()
        {
            var roles = _servicioRoles.obtenerRolesDTO();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerRolPorId(int id)
        {
            var rol = _servicioRoles.obtenerRolesPorId(id);
            if (rol == null)
            {
                return NotFound();
            }
            return Ok(rol);
        }
    }
}
