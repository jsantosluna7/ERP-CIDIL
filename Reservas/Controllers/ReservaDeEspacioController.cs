using ERP.Data.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly DbErpContext _context;

        public ReservaDeEspacioController(IServicioReservaDeEspacio servicioReservaDeEspacio, DbErpContext context)
        {
            _servicioReservaDeEspacio = servicioReservaDeEspacio;
            _context = context;
        }

        [HttpGet("obtener-cantidad-reserva-espacios")]
        public async Task<IActionResult> cantidadReservaEspacios()
        {
            var totalReservaEspacios = await _context.SolicitudReservaDeEspacios.Where(r => r.IdEstado == 2).CountAsync();
            // Devolver la cantidad de usuarios

            var respuesta = new
            {
                totalReservaEspacios
            };
            return Ok(respuesta);
        }

        // Método para obtener todas las reservas
        [HttpGet("obtener-reservas")]
        public async Task<IActionResult> ObtenerReservas([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            var reservas = await _servicioReservaDeEspacio.ObtenerReservas(pagina, tamanoPagina);
            if (reservas == null)
            {
                return NotFound("No se encontraron reservas de espacios.");
            }

            var totalEspacios = await _context.InventarioEquipos.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalEspacios / (double)tamanoPagina);

            var respuesta = new
            {
                paginacion = new
                {
                    paginaActual = pagina,
                    tamanoPagina,
                    totalEspacios,
                    totalPaginas
                },
                datos = reservas
            };

            return Ok(respuesta);
        }

        // Método para obtener una reserva por id
        [HttpGet("obtener-reservas-pisos/{piso}")]
        public async Task<IActionResult> ObtenerReservaPorPiso(int piso)
        {
            var reserva = await _servicioReservaDeEspacio.ObtenerReservasDeEspacioPorPiso(piso);
            if (reserva == null)
            {
                return NotFound($"No se encontró la reserva en el piso {piso}.");
            }
            return Ok(reserva);
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

            if (!User.TieneRol("1", "2"))
            {
                return Unauthorized("No tienes permiso para acceder a esta información");
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

            if (!User.TieneRol("1"))
            {
                return Unauthorized("No tienes permiso para acceder a esta información");
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

            if (!User.TieneRol("1", "2"))
            {
                return Unauthorized("No tienes permiso para acceder a esta información");
            }
            // Devolver una respuesta exitosa
            return Ok($"Usuario con ID {id} desactivado");
        }
    }
}
