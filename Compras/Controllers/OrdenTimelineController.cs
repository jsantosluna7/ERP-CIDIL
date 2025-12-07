using Compras.Abstraccion.Servicios;
using Compras.DTO.OrdenItemDTO;
using Compras.DTO.OrdenTimelineDTO;
using Compras.Implementaciones.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenTimelineController : ControllerBase
    {
        private readonly IServicioOrdenTimeline _servicioOrdenTimeline;
        public OrdenTimelineController(IServicioOrdenTimeline servicioOrdenTimeline)
        {
            _servicioOrdenTimeline = servicioOrdenTimeline;
        }

        [HttpGet]
        public async Task<IActionResult> OrdenTimeline()
        {
            var resultado = await _servicioOrdenTimeline.OrdenTimeline();
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("orden-timeline-id")]
        public async Task<IActionResult> OrdenTimelineId([FromQuery] int id)
        {
            var resultado = await _servicioOrdenTimeline.OrdenTimelineId(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("orden-timeline-orden")]
        public async Task<IActionResult> OrdenTimelinePorOrdenId([FromQuery] int ordenTimelineId)
        {
            var resultado = await _servicioOrdenTimeline.OrdenTimelinePorOrdenId(ordenTimelineId);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }
    }
}
