using Compras.Abstraccion.Servicios;
using Compras.DTO.ComentariosOrdenDTO;
using Compras.DTO.OrdenItemDTO;
using Compras.Implementaciones.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenItemController : ControllerBase
    {
        private readonly IServicioOrdenItem _servicioOrdenItem;
        public OrdenItemController(IServicioOrdenItem servicioOrdenItem)
        {
            _servicioOrdenItem = servicioOrdenItem;
        }

        [HttpGet]
        public async Task<IActionResult> OrdenItem()
        {
            var resultado = await _servicioOrdenItem.OrdenItem();
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("orden-item-id")]
        public async Task<IActionResult> OrdenItemId([FromQuery] int id)
        {
            var resultado = await _servicioOrdenItem.OrdenItemId(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("orden-item-orden")]
        public async Task<IActionResult> OrdenItemPorOrdenId([FromQuery] int ordenItemId)
        {
            var resultado = await _servicioOrdenItem.OrdenItemPorOrdenId(ordenItemId);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPost]
        public async Task<IActionResult> CrearOrdenItem([FromBody] CrearOrdenItemDTO ordenItemDTO)
        {
            var resultado = await _servicioOrdenItem.CrearOrdenItem(ordenItemDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPatch]
        public async Task<IActionResult> ActualizarOrdenItem([FromQuery] int id, [FromBody] CrearOrdenItemDTO ordenItemDTO)
        {
            var resultado = await _servicioOrdenItem.ActualizarOrdenItem(id, ordenItemDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar([FromQuery] int id)
        {
            var resultado = await _servicioOrdenItem.Eliminar(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }
    }
}
