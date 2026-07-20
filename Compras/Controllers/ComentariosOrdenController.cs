using Compras.Abstraccion.Servicios;
using Compras.DTO.ComentariosOrdenDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosOrdenController : ControllerBase
    {
        private readonly IServicioComentariosOrden _servicioComentariosOrden;

        public ComentariosOrdenController(IServicioComentariosOrden servicioComentariosOrden)
        {
            _servicioComentariosOrden = servicioComentariosOrden;
        }

        [HttpGet]
        public async Task<IActionResult> ComentariosOrden()
        {
            var resultado = await _servicioComentariosOrden.ComentariosOrden();
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("comentarios-orden-id")]
        public async Task<IActionResult> ComentariosOrdenId([FromQuery] int id)
        {
            var resultado = await _servicioComentariosOrden.ComentariosOrdenId(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("comentarios-por-orden")]
        public async Task<IActionResult> ComentariosPorOrden([FromQuery] int ordenId)
        {
            var resultado = await _servicioComentariosOrden.ComentariosOrdenPorOrdenId(ordenId);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("comentarios-por-item")]
        public async Task<IActionResult> ComentariosPorItem([FromQuery] int itemId)
        {
            var resultado = await _servicioComentariosOrden.ComentariosOrdenPorItemId(itemId);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("comentarios-por-usuario")]
        public async Task<IActionResult> ComentariosPorUsuario([FromQuery] int usuarioId)
        {
            var resultado = await _servicioComentariosOrden.ComentariosOrdenPorUsuarioId(usuarioId);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpPost]
        public async Task<IActionResult> CrearComentariosOrden([FromBody] CrearComentariosOrdenDTO comentarioDTO)
        {
            var resultado = await _servicioComentariosOrden.CrearComentariosOrden(comentarioDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpPatch]
        public async Task<IActionResult> ActualizarComentariosOrden([FromQuery] int id, [FromBody] CrearComentariosOrdenDTO comentarioDTO)
        {
            var resultado = await _servicioComentariosOrden.ActualizarComentariosOrden(id, comentarioDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarComentariosOrden([FromQuery] int id)
        {
            var resultado = await _servicioComentariosOrden.Eliminar(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(resultado.MensajeError);
            }
            return Ok(resultado.Valor);
        }
      }
    }
