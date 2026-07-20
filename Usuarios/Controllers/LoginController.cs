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
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPost("verificar-otp")]
        public async Task<IActionResult> VerificarOtp([FromBody] VerificarOtpDTO verificarOtp)
        {
            //Aquí puedes llamar al servicio para verificar el OTP
            var resultado = await _servicioLogin.verificarOtp(verificarOtp);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPost("iniciar-sesion")]
        public async Task<IActionResult> IniciarSecion([FromBody] Login login)
        {
            //Aquí puedes llamar al servicio para iniciar seción
            var resultado = await _servicioLogin.IniciarSecion(login);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }
    }
}
