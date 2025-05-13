using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaDeEspacioController : ControllerBase
    {
        private readonly IServicioReservaDeEspacio _servicioReservaDeEspacio;
        public ReservaDeEspacioController(IServicioReservaDeEspacio servicioReservaDeEspacio)
        {
            _servicioReservaDeEspacio = servicioReservaDeEspacio;
        }

        // Método para obtener todas las reservas
        [HttpGet("obtener-reservas")]
        public async Task<IActionResult> ObtenerReservas()
        {
            var reservas = await _servicioReservaDeEspacio.ObtenerReservas();
            if (reservas == null)
            {
                return NotFound("No se encontraron reservas de espacios.");
            }
            return Ok(reservas);
        }

        // Método para obtener todas las solicitudes de reserva
        [HttpGet("obtener-solicitudes-reservas")]
        public async Task<IActionResult> ObtenerSolicitudesReservas()
        {
            var solicitudes = await _servicioReservaDeEspacio.ObtenerSolicitudesReservas();
            if (solicitudes == null)
            {
                return NotFound("No se encontraron solicitudes de reserva.");
            }
            return Ok(solicitudes);
        }

        // Método para obtener una reserva por id
        [HttpGet("obtener-reservas/{id}")]
        public async Task<IActionResult> ObtenerReservaPorId(int id)
        {
            var reserva = await _servicioReservaDeEspacio.ObtenerReservaPorId(id);
            if (reserva == null)
            {
                return NotFound($"No se encontró la reserva con id {id}.");
            }
            return Ok(reserva);
        }

        // Método para obtener una solicitud de reserva por id
        [HttpGet("obtener-solicitudes-reservas/{id}")]
        public async Task<IActionResult> ObtenerSolicitudReservaPorId(int id)
        {
            var solicitud = await _servicioReservaDeEspacio.ObtenerSolicitudReservaPorId(id);
            if (solicitud == null)
            {
                return NotFound($"No se encontró la solicitud de reserva con id {id}.");
            }
            return Ok(solicitud);
        }

        // Método para crear una reserva
        [HttpPost("crear-reserva")]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaDeEspacioDTO crearReservaDeEspacioDTO)
        {
            var solicitud = await _servicioReservaDeEspacio.CrearReserva(crearReservaDeEspacioDTO);
            if (solicitud == null)
            {
                return BadRequest("Ya existe una reserva en el tiempo que definiste.");
            }
            return Ok(solicitud);
        }

        // Método para crear una solicitud de reserva
        [HttpPost("crear-solicitud-reserva")]
        public async Task<IActionResult> CrearSolicitudReserva([FromBody] CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO)
        {
            var solicitud = await _servicioReservaDeEspacio.SolicitarCrearReserva(crearSolicitudDeReservaDTO);
            if (solicitud == null)
            {
                return BadRequest("Ya existe una reserva en el tiempo que definiste.");
            }
            return Ok(solicitud);
        }

        // Método para editar una reserva
        [HttpPut("editar-reserva/{id}")]
        public async Task<IActionResult> EditarReserva(int id, [FromBody] ActualizarReservaDeEspacioDTO actualizarReservaDeEspacioDTO)
        {
            var reserva = await _servicioReservaDeEspacio.EditarReserva(id, actualizarReservaDeEspacioDTO);
            if (reserva == null)
            {
                return NotFound($"No se encontró la reserva con id {id} y/o ya existe una reserva en el tiempo que definiste.");
            }
            return Ok(reserva);
        }

        // Método para editar una solicitud de reserva
        [HttpPut("editar-solicitud-reserva/{id}")]
        public async Task<IActionResult> EditarSolicitudReserva(int id, [FromBody] ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO)
        {
            var solicitud = await _servicioReservaDeEspacio.EditarSolicitudReserva(id, actualizarSolicitudDeReservaDTO);
            if (solicitud == null)
            {
                return NotFound($"No se encontró la solicitud de reserva con id {id} y/o ya existe una reserva en el tiempo que definiste.");
            }
            return Ok(solicitud);
        }

        // Método para cancelar una reserva
        [HttpDelete("cancelar-reserva/{id}")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var resultado = await _servicioReservaDeEspacio.CancelarReserva(id);
            if (resultado == null)
            {
                return NotFound($"No se encontró la reserva con id {id}.");
            }
            return Ok(resultado);
        }

        // Método para cancelar una solicitud de reserva
        [HttpDelete("cancelar-solicitud-reserva/{id}")]
        public async Task<IActionResult> CancelarSolicitudReserva(int id)
        {
            var resultado = await _servicioReservaDeEspacio.CancelarSolicitudReserva(id);
            if (resultado == null)
            {
                return NotFound($"No se encontró la solicitud de reserva con id {id}.");
            }
            return Ok(resultado);
        }
    }
}
