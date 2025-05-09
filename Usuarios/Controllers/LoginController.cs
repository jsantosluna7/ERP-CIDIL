using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.LoginDTO;
using Usuarios.Modelos;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IServicioLogin _servicioLogin;
        public LoginController(IServicioLogin servicioLogin)
        {
            _servicioLogin = servicioLogin;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] CrearRegistroDTO crearRegistroDTO)
        {
            //Aquí puedes llamar al servicio para registrar el usuario
            var resultado = await _servicioLogin.RegistrarUsuario(crearRegistroDTO);
            if (resultado == null)
            {
                return BadRequest("El correo y/o la matricula ya existen, la contraseña debe ser mayor a 8 caracteres.");
            }
            return Ok(resultado);
        }

        [HttpPost("iniciar-sesion")]
        public async Task<IActionResult> IniciarSecion([FromBody] Login login)
        {
            //Aquí puedes llamar al servicio para iniciar seción
            var resultado = await _servicioLogin.IniciarSecion(login);
            if (resultado == null)
            {
                return BadRequest("El correo y/o la contraseña son incorrectos, la contraseña debe ser mayor a 8 caracteres.");
            }
            return Ok(resultado);
        }
    }
}
