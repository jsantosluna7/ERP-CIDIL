using Compras.Abstraccion.Servicios;
using Compras.DTO.OrdenesDTO;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {
        private readonly IServicioOrdenes _servicioOrdenes;
        public OrdenesController(IServicioOrdenes servicioOrdenes)
        {
            _servicioOrdenes = servicioOrdenes;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerOrdenes()
        {
            var resultado = await _servicioOrdenes.OrdenesAll();
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("ordenes-id")]
        public async Task<IActionResult> ObtenerPorId([FromQuery] int id)
        {
            var resultado = await _servicioOrdenes.ObtenerPorId(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] CrearOrdenesDTO ordenesDTO)
        {
            var resultado = await _servicioOrdenes.CrearOrdenes(ordenesDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPatch]
        public async Task<IActionResult> ActualizarOrdenes([FromQuery] int id,[FromBody] CrearOrdenesDTO ordenesDTO)
        {
            var resultado = await _servicioOrdenes.ActualizarOrdenes(id, ordenesDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar([FromQuery] int id)
        {
            var resultado = await _servicioOrdenes.Eliminar(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }
    }
}
