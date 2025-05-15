using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOReservaDeEspacio;
using Reservas.DTO.DTOSolicitudDeReserva;
using Reservas.Modelos;

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

        [HttpPatch("{id}")]
        public async Task<IActionResult> DesactivarReservaDeEspacio(int id)
        {
            // Llamar al servicio para desactivar un equipo por su ID
            var prestamoEquiposDesactivado = await _servicioReservaDeEspacio.desactivarReservaDeEspacio(id);
            // Verificar si el equipo fue desactivado
            if (prestamoEquiposDesactivado == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }
            // Devolver una respuesta exitosa
            return Ok($"Usuario con ID {id} desactivado");
        }
    }
}
