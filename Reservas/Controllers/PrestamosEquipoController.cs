using ERP.Data.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOPrestamosEquipo;
using Reservas.Implementaciones.Servicios;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosEquipoController : ControllerBase
    {
        private readonly IServicioPrestamosEquipo _prestamosEquipo;
        private readonly DbErpContext _context;

        public PrestamosEquipoController(IServicioPrestamosEquipo prestamosEquipo, DbErpContext context)
        {
            this._prestamosEquipo = prestamosEquipo;
            _context = context;
        }

        [HttpGet("obtener-cantidad-prestamos-equipos")]
        public async Task<IActionResult> cantidadPrestamosEquipos()
        {
            var totalPrestamosEquipos = await _context.SolicitudPrestamosDeEquipos.Where(p => p.IdEstado == 2).CountAsync();
            // Devolver la cantidad de usuarios

            var respuesta = new
            {
                totalPrestamosEquipos
            };
            return Ok(respuesta);
        }


        [HttpGet]
        public async Task<IActionResult?> GetPrestamosEquipo([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            var resultado = await _prestamosEquipo.GetPrestamosEquipo(pagina, tamanoPagina);
            if (resultado == null)
            {
                return NotFound("Lista de Prestamos no encontrada");
            }

            var totalPrestamos = await _context.PrestamosEquipos.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalPrestamos / (double)tamanoPagina);

            var respuesta = new
            {
                paginacion = new
                {
                    paginaActual = pagina,
                    tamanoPagina,
                    totalPrestamos,
                    totalPaginas
                },
                datos = resultado
            };
            return Ok(respuesta);
        }

        [HttpGet("mis-equipos")]
        public async Task<IActionResult> ObtenerSolicitudEquiposUsuario([FromQuery] int usuario)
        {
            var resultado = await _prestamosEquipo.ObtenerEquiposUsuario(usuario);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPost]
        public async Task<IActionResult?> Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO)
        {
            var resultado = await _prestamosEquipo.Crear(crearPrestamosEquipoDTO);
            if (resultado == null)
            {
                return NotFound("No se pudo crear su solicitud");
            }
            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult?> Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO)
        {
            var resultado = await _prestamosEquipo.Actualizar(id, actualizarPrestamosEquipoDTO);
            if (resultado == null)
            {
                return NotFound("No se pudo actualizar su solicitud");
            }

            if (!User.TieneRol("1", "2"))
            {
                return Unauthorized("No tienes permiso para acceder a esta información");
            }
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult?> GetById(int id)
        {
            var resultado = await _prestamosEquipo.GetById(id);
            if (resultado == null)
            {
                return NotFound("La Lista de Prestamos de Equipos no fue encontrada");
            }
            return Ok(resultado);
            
        }

        [HttpDelete("{id}")]
        public async  Task<IActionResult?> DeleteById(int id)
        {
            await _prestamosEquipo.Eliminar(id);

            // JWT Authorization
            if (!User.TieneRol("1"))
            {
                return Unauthorized("No tienes permiso para acceder a esta información");
            }
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> DesactivarPrestamoEquipos(int id)
        {
            // Llamar al servicio para desactivar un equipo por su ID
            var prestamoEquiposDesactivado = await _prestamosEquipo.DesactivarPrestamoEquipos(id);
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
