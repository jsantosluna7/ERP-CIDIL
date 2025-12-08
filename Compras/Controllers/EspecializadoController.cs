using Compras.Abstraccion.Servicios;
using Compras.DTO.EspecializadosDTO;
using Compras.DTO.OrdenesDTO;
using Compras.Implementaciones.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecializadoController : ControllerBase
    {
        private readonly IServicioEspecializado _servicioEspecializado;

        public EspecializadoController(IServicioEspecializado servicioEspecializado)
        {
            _servicioEspecializado = servicioEspecializado;
        }

        [HttpPost("{id}/actualizar-estado")]
        public async Task<IActionResult> ActualizarEstadoOrden(int id, [FromBody] ActualizarEstadoOrdenDTO actualizarEstadoOrdenDTO)
        {
            var resultado = await _servicioEspecializado.ActualizarEstadoOrden(id, actualizarEstadoOrdenDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpPost("items/{itemId}/actualizar-recepcion")]
        public async Task<IActionResult> ActualizarRecepcion(int itemId, [FromBody] ActualizarItemRecepcionDTO actualizarItemRecepcionDTO)
        {
            var resultado = await _servicioEspecializado.ActualizarItemRecepcion(itemId, actualizarItemRecepcionDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("{id}/timeline")]
        public async Task<IActionResult> ObtenerTimeline(int id)
        {
            var resultado = await _servicioEspecializado.ObtenerTimeline(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("{id}/items")]
        public async Task<IActionResult> ObtenerItems(int id)
        {
            var resultado = await _servicioEspecializado.ObtenerItems(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }
    }
}
