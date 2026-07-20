using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservas.Abstraccion.Servicios;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly IServicioEstado _servicioEstado;

        public EstadoController(IServicioEstado servicioEstado)
        {
            _servicioEstado = servicioEstado;
        }

        [HttpGet]
        public async Task<IActionResult?> GetEstado()
        {
            var resultado = await _servicioEstado.GetEstado();
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }

            return Ok(resultado.Valor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult?> GetById(int id)
        {
            var resultado = await _servicioEstado.GetById(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }

            return Ok(resultado.Valor);
        }
    }
}
