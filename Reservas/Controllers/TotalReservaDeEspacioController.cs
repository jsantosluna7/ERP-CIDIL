using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservas.Abstraccion.Servicios;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalReservaDeEspacioController : ControllerBase
    {
        private readonly IServicioTotalReservaDeEspacio _servicio;

        public TotalReservaDeEspacioController(IServicioTotalReservaDeEspacio servicio)
        {
            _servicio = servicio;
        }

        [HttpGet("{idUsuario}")]
        public async Task<IActionResult> ObtenerReservasDeEspacioUsuario(
            int idUsuario,
            [FromQuery] int? idEstado = null)
        {
            var resultado = await _servicio.ObtenerTodasLasReservasDeEspacioDelUsuario(idUsuario, idEstado);

            if (!resultado.esExitoso)
            {
                bool esValidacion = resultado.MensajeError?.Contains("no es válido") ?? false;
                return esValidacion
                    ? BadRequest(new { mensaje = resultado.MensajeError })
                    : NotFound(new { mensaje = resultado.MensajeError });
            }

            return Ok(new
            {
                IdUsuario = idUsuario,
                FiltroEstado = idEstado.HasValue ? idEstado.ToString() : "Todas",
                Total = resultado.Valor!.Count,
                Reservas = resultado.Valor
            });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodasLasReservasDeEspacios(
            [FromQuery] int? idEstado = null,
            [FromQuery] string? busqueda = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 20)
        {
            var resultado = await _servicio.ObtenerTodasLasReservasDeEspacios(
                idEstado, busqueda, pagina, tamanoPagina);

            var conteo = await _servicio.ObtenerConteoReservasDeEspacios();

            if (!resultado.esExitoso)
            {
                bool esValidacion = resultado.MensajeError?.Contains("válido") == true
                                 || resultado.MensajeError?.Contains("página") == true;

                return esValidacion
                    ? BadRequest(new { mensaje = resultado.MensajeError })
                    : NotFound(new { mensaje = resultado.MensajeError });
            }

            return Ok(new
            {
                Pagina             = pagina,
                TamanoPagina       = tamanoPagina,
                FiltroEstado       = idEstado.HasValue ? idEstado.ToString() : "Todos",
                Busqueda           = busqueda ?? "Sin filtro",
                Total              = conteo.Valor!.TotalSolicitudes,
                ReservasDeEspacios = resultado.Valor
            });
        }

        [HttpGet("conteo")]
        public async Task<IActionResult> ObtenerConteoReservasDeEspacios()
        {
            var resultado = await _servicio.ObtenerConteoReservasDeEspacios();
            return Ok(resultado.Valor);
        }

    }
}
