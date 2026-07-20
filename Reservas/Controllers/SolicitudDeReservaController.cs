using ERP.Data.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Implementaciones.Servicios;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudDeReservaController : ControllerBase
    {
        private readonly IServicioSolicitudDeReserva _servicioSolicitudDeReserva;
        private readonly DbErpContext _dbContext;
        public SolicitudDeReservaController(IServicioSolicitudDeReserva servicioSolicitudDeReserva, DbErpContext dbContext)
        {
            _servicioSolicitudDeReserva = servicioSolicitudDeReserva;
            _dbContext = dbContext;
        }

        // Método para obtener todas las solicitudes de reserva
        [HttpGet("obtener-solicitudes-reservas")]
        public async Task<IActionResult> ObtenerSolicitudesReservas([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            var resultado = await _servicioSolicitudDeReserva.ObtenerSolicitudesReservas(pagina, tamanoPagina);
            if (resultado == null)
            {
                return NotFound("Lista de Inventario de Equipos no encontrada");
            }

            var totalInventario = await _dbContext.SolicitudReservaDeEspacios.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalInventario / (double)tamanoPagina);

            var respuesta = new
            {
                paginacion = new
                {
                    paginaActual = pagina,
                    tamanoPagina,
                    totalInventario,
                    totalPaginas
                },
                datos = resultado
            };
            return Ok(respuesta);
        }

        // Método para obtener una solicitud de reserva por piso
        [HttpGet("obtener-solicitudes-reservas-piso/{piso}")]
        public async Task<IActionResult> ObtenerSolicitudReservaPorPiso(int piso)
        {
            var solicitud = await _servicioSolicitudDeReserva.ObtenerSolicitudesReservasPorPiso(piso);
            if (solicitud == null)
            {
                return NotFound($"No se encontró la solicitud de reserva en el piso {piso}.");
            }
            return Ok(solicitud);
        }

        // Método para obtener una solicitud de reserva por id
        [HttpGet("obtener-solicitudes-reservas/{id}")]
        public async Task<IActionResult> ObtenerSolicitudReservaPorId(int id)
        {
            var solicitud = await _servicioSolicitudDeReserva.ObtenerSolicitudReservaPorId(id);
            if (!solicitud.esExitoso)
            {
                return BadRequest(new { error = solicitud.MensajeError });
            }
            return Ok(solicitud.Valor);
        }

        [HttpGet("mis-solicitudes-espacios")]
        public async Task<IActionResult> ObtenerSolicitudEspaciosUsuario([FromQuery] int usuario)
        {
            var resultado = await _servicioSolicitudDeReserva.ObtenerSolicitudEspaciosUsuario(usuario);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        // Método para crear una solicitud de reserva
        [HttpPost("crear-solicitud-reserva")]
        public async Task<IActionResult> CrearSolicitudReserva([FromBody] CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO)
        {
            var solicitud = await _servicioSolicitudDeReserva.SolicitarCrearReserva(crearSolicitudDeReservaDTO);
            if (!solicitud.esExitoso)
            {
                return BadRequest(new { error = solicitud.MensajeError });
            }
            return Ok(solicitud.Valor);
        }

        // Método para editar una solicitud de reserva
        [HttpPut("editar-solicitud-reserva/{id}")]
        public async Task<IActionResult> EditarSolicitudReserva(int id, [FromBody] ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO)
        {
            var solicitud = await _servicioSolicitudDeReserva.EditarSolicitudReserva(id, actualizarSolicitudDeReservaDTO);
            if (!solicitud.esExitoso)
            {
                return BadRequest(new { error = solicitud.MensajeError });
            }

            // Verificar si el usuario tiene el rol adecuado
            if (!User.TieneRol("1", "2"))
            {
                return Unauthorized("No tienes permiso para acceder a esta información");
            }
            return Ok(solicitud.Valor);
        }

        // Método para cancelar una solicitud de reserva
        [HttpDelete("cancelar-solicitud-reserva/{id}")]
        public async Task<IActionResult> CancelarSolicitudReserva(int id)
        {
            var resultado = await _servicioSolicitudDeReserva.CancelarSolicitudReserva(id);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }

            return Ok(resultado.Valor);
        }
    }
}
