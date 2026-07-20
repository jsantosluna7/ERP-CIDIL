using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Abstraccion.Servicios;
using Usuarios.Modelos;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetearContrasenaController : ControllerBase
    {
        private readonly IServicioResetPassword _servicioResetPassword;
        public ResetearContrasenaController(IServicioResetPassword servicioResetPassword)
        {
            _servicioResetPassword = servicioResetPassword;
        }

        [HttpPost("olvide-contrasena")]
        public async Task<IActionResult> OlvideContrasena([FromBody] OlvideContrasena olvideContrasena)
        {
            var resultado = await _servicioResetPassword.OlvideContrasena(olvideContrasena);
            if (resultado == null)
            {
                return NotFound("El correo institucional no existe.");
            }
            return Ok();
        }

        [HttpPost("restablecer-contrasena")]
        public async Task<IActionResult> RestablecerContrasena([FromBody] ResetearContrasena resetearContrasena)
        {
            var resultado = await _servicioResetPassword.RestablecerContrasena(resetearContrasena);
            if (resultado == null)
            {
                return NotFound("El token no es válido, ha expirado o ingresaste la contraseña similar a la anterior, la contraseña debe ser mayor a 8 caracteres.");
            }
            return Ok();
        }
    }
}
