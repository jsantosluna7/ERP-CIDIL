using Compras.Abstraccion.Servicios;
using Compras.DTO.EstadosTimelineDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosTimelineController : ControllerBase
    {
        private readonly IServicioEstadosTimeline _servicioEstadosTimeline;

        public EstadosTimelineController(IServicioEstadosTimeline servicioEstadosTimeline)
        {
            _servicioEstadosTimeline = servicioEstadosTimeline;
        }

        [HttpGet]
        public async Task<IActionResult> EstadosTimeline()
        {
            var resultado = await _servicioEstadosTimeline.EstadosTimeline();
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("estados-timeline-id")]
        public async Task<IActionResult> EstadosTimelineId([FromQuery] int id)
        {
            var resultado = await _servicioEstadosTimeline.EstadosTimelineId(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPost]
        public async Task<IActionResult> CrearEstadosTimeline([FromBody] EstadosTimelineDTO estadosTimelineDTO)
        {
            var resultado = await _servicioEstadosTimeline.CrearEstadosTimeline(estadosTimelineDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPatch]
        public async Task<IActionResult> ActualizarEstadosTimeline([FromQuery] int id, [FromBody] EstadosTimelineDTO estadosTimelineDTO)
        {
            var resultado = await _servicioEstadosTimeline.ActualizarEstadosTimeline(id, estadosTimelineDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar([FromQuery] int id)
        {
            var resultado = await _servicioEstadosTimeline.Eliminar(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }
    }
}
