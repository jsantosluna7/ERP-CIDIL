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
            if (resultado == null)
            {
                return NotFound("Lista de estado no encontrada");
            }
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult?> GetById(int id)
        {
            var resultado = await _servicioEstado.GetById(id);
            if (resultado == null)
            {
                return NotFound("Lista de Estado No encontrada");
            }
            return Ok(resultado);
        }
    }
}
