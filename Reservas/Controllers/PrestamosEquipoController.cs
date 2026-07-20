using ERP.Data.Modelos;
using Microsoft.AspNetCore.Mvc;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOPrestamosEquipo;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosEquipoController : ControllerBase
    {
        private readonly IServicioPrestamosEquipo _servicio;

        public PrestamosEquipoController(IServicioPrestamosEquipo servicio)
        {
            _servicio = servicio;
        }

        /// <summary>
        /// Obtiene préstamos con filtros opcionales combinables.
        /// </summary>
        /// <param name="estado">null = todos | pendiente | activo | atrasado</param>
        /// <param name="idUsuario">Si se indica, devuelve solo los de ese usuario</param>
        /// <param name="pagina">Página (default: 1)</param>
        /// <param name="tamanoPagina">Registros por página (default: 20)</param>
        // GET /api/prestamos-equipo
        // GET /api/prestamos-equipo?estado=pendiente
        // GET /api/prestamos-equipo?estado=activo&pagina=2
        // GET /api/prestamos-equipo?idUsuario=3
        [HttpGet]
        public async Task<IActionResult> ObtenerPrestamos(
            [FromQuery] string? estado = null,
            [FromQuery] int? idUsuario = null,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 20)
        {
            var resultado = await _servicio.ObtenerPrestamos(estado, idUsuario, pagina, tamanoPagina);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });

            return Ok(resultado.Valor);
        }

        // GET /api/prestamos-equipo/resumen
        [HttpGet("resumen")]
        public async Task<IActionResult> ObtenerResumen()
        {
            var resultado = await _servicio.ObtenerResumen();
            return Ok(resultado.Valor);
        }

        // GET /api/prestamos-equipo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _servicio.ObtenerPorId(id);
            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(resultado.Valor);
        }

        /// <summary>
        /// Procesa una solicitud o una extensión según TipoAccion.
        /// TipoAccion = "solicitud" → requiere IdSolicitud
        /// TipoAccion = "extension" → requiere IdExtension
        /// </summary>
        // POST /api/prestamos-equipo/procesar
        [HttpPost("procesar")]
        public async Task<IActionResult> Procesar([FromBody] ProcesarPrestamosEquipoDTO dto)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized("No tienes permiso para realizar esta acción.");

            var resultado = await _servicio.Procesar(dto);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });

            return Ok(resultado.Valor);
        }

        // PATCH /api/prestamos-equipo/5/marcar-devuelto
        [HttpPatch("{id}/marcar-devuelto")]
        public async Task<IActionResult> MarcarDevuelto(int id, [FromBody] MarcarDevueltoDTO dto)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized("No tienes permiso para realizar esta acción.");

            var resultado = await _servicio.MarcarDevuelto(id, dto);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });

            return Ok(resultado.Valor);
        }

        // DELETE /api/prestamos-equipo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (!User.TieneRol("1"))
                return Unauthorized("No tienes permiso para realizar esta acción.");

            var resultado = await _servicio.Eliminar(id);
            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });

            return Ok(new { mensaje = "Préstamo eliminado correctamente." });
        }

        // ── Extensiones ───────────────────────────────────────────────────────────

        // GET /api/prestamos-equipo/extensiones/pendientes
        [HttpGet("extensiones/pendientes")]
        public async Task<IActionResult> ObtenerExtensionsPendientes()
        {
            var resultado = await _servicio.ObtenerExtensionsPendientes();
            return Ok(resultado.Valor);
        }

        // GET /api/prestamos-equipo/5/extensiones
        [HttpGet("{idPrestamo}/extensiones")]
        public async Task<IActionResult> ObtenerExtensionsPorPrestamo(int idPrestamo)
        {
            var resultado = await _servicio.ObtenerExtensionsPorPrestamo(idPrestamo);
            return Ok(resultado.Valor);
        }

        // POST /api/prestamos-equipo/5/extensiones
        [HttpPost("{idPrestamo}/extensiones")]
        public async Task<IActionResult> SolicitarExtension(int idPrestamo, [FromBody] CrearExtensionDTO dto)
        {
            var resultado = await _servicio.SolicitarExtension(idPrestamo, dto);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });

            return Ok(resultado.Valor);
        }
    }
}