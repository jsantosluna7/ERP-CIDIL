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

        // Método para obtener una solicitud de reserva por id
        [HttpGet("obtener-solicitudes-reservas/{id}")]
        public async Task<IActionResult> ObtenerSolicitudReservaPorId(int id)
        {
            var solicitud = await _servicioSolicitudDeReserva.ObtenerSolicitudReservaPorId(id);
            if (solicitud == null)
            {
                return NotFound($"No se encontró la solicitud de reserva con id {id}.");
            }
            return Ok(solicitud);
        }

        // Método para crear una solicitud de reserva
        [HttpPost("crear-solicitud-reserva")]
        public async Task<IActionResult> CrearSolicitudReserva([FromBody] CrearSolicitudDeReservaDTO crearSolicitudDeReservaDTO)
        {
            var solicitud = await _servicioSolicitudDeReserva.SolicitarCrearReserva(crearSolicitudDeReservaDTO);
            if (solicitud == null)
            {
                return BadRequest("Ya existe una reserva en el tiempo que definiste.");
            }
            return Ok(solicitud);
        }

        // Método para editar una solicitud de reserva
        [HttpPut("editar-solicitud-reserva/{id}")]
        public async Task<IActionResult> EditarSolicitudReserva(int id, [FromBody] ActualizarSolicitudDeReservaDTO actualizarSolicitudDeReservaDTO)
        {
            var solicitud = await _servicioSolicitudDeReserva.EditarSolicitudReserva(id, actualizarSolicitudDeReservaDTO);
            if (solicitud == null)
            {
                return NotFound($"No se encontró la solicitud de reserva con id {id} y/o ya existe una reserva en el tiempo que definiste.");
            }
            return Ok(solicitud);
        }

        // Método para cancelar una solicitud de reserva
        [HttpDelete("cancelar-solicitud-reserva/{id}")]
        public async Task<IActionResult> CancelarSolicitudReserva(int id)
        {
            var resultado = await _servicioSolicitudDeReserva.CancelarSolicitudReserva(id);
            if (resultado == null)
            {
                return NotFound($"No se encontró la solicitud de reserva con id {id}.");
            }
            return Ok(resultado);
        }
    }
}
