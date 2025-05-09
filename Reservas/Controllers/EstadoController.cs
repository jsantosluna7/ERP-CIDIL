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
        public IActionResult GetEstado()
        {
            var resultado = _servicioEstado.GetEstado();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var resultado = _servicioEstado.GetById(id);
            return Ok(resultado);
        }
    }
}
