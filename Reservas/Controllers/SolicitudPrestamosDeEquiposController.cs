using ERP.Data.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOSolicitudDeEquipos;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudPrestamosDeEquiposController : ControllerBase
    {
        private readonly IServicioSolicitudPrestamosDeEquipos _servicio;

        public SolicitudPrestamosDeEquiposController(IServicioSolicitudPrestamosDeEquipos servicio)
        {
            _servicio = servicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            var resultado = await _servicio.ObtenerTodas(pagina, tamanoPagina);
            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(new
            {
                paginacion = new { paginaActual = pagina, tamanoPagina },
                datos = resultado.Valor
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _servicio.ObtenerPorId(id);
            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(resultado.Valor);
        }

        [HttpGet("mis-solicitudes")]
        public async Task<IActionResult> ObtenerPorUsuario([FromQuery] int usuario)
        {
            var resultado = await _servicio.ObtenerPorUsuario(usuario);
            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(resultado.Valor);
        }

        [HttpPost]
        public async Task<IActionResult> CrearMultiples([FromBody] CrearSolicitudPrestamosDeEquiposDTO dto)
        {
            var resultado = await _servicio.CrearMultiples(dto);

            // Si todos fueron exitosos → 200 OK
            // Si alguno falló → 422 Unprocessable Entity para que el frontend sepa que hay errores por ítem
            if (!resultado.TodosExitosos)
                return UnprocessableEntity(resultado);

            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarSolicitudPrestamosDeEquiposDTO dto)
        {
            var resultado = await _servicio.Actualizar(id, dto);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });

            return Ok(resultado.Valor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _servicio.Eliminar(id);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });

            return Ok(new { mensaje = "Solicitud cancelada correctamente." });
        }
    }
    
}
