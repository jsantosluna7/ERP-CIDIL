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
        private readonly IServicioPrestamosEquipo _servicio;

        public PrestamosEquipoController(IServicioPrestamosEquipo servicio)
        {
            _servicio = servicio;
        }

        // GET /api/prestamos-equipo?pagina=1&tamanoPagina=20
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            var resultado = await _servicio.ObtenerTodos(pagina, tamanoPagina);
            return Ok(resultado.Valor);
        }

        // GET /api/prestamos-equipo/resumen — contadores para el dashboard
        [HttpGet("resumen")]
        public async Task<IActionResult> ObtenerResumen()
        {
            var resultado = await _servicio.ObtenerResumen();
            return Ok(resultado.Valor);
        }

        // GET /api/prestamos-equipo/pendientes
        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerPendientes()
        {
            var resultado = await _servicio.ObtenerPendientes();
            return Ok(resultado.Valor);
        }

        // GET /api/prestamos-equipo/activos
        [HttpGet("activos")]
        public async Task<IActionResult> ObtenerActivos()
        {
            var resultado = await _servicio.ObtenerActivos();
            return Ok(resultado.Valor);
        }

        // GET /api/prestamos-equipo/atrasados
        [HttpGet("atrasados")]
        public async Task<IActionResult> ObtenerAtrasados()
        {
            var resultado = await _servicio.ObtenerAtrasados();
            return Ok(resultado.Valor);
        }

        // GET /api/prestamos-equipo/mis-equipos?usuario=3
        [HttpGet("mis-equipos")]
        public async Task<IActionResult> ObtenerPorUsuario([FromQuery] int usuario)
        {
            var resultado = await _servicio.ObtenerPorUsuario(usuario);
            if (!resultado.esExitoso)
                return NotFound(new { error = resultado.MensajeError });
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

        // PATCH /api/prestamos-equipo/5/aprobar-rechazar
        // Body: { "idUsuarioAprobador": 1, "aprobado": true, "comentarioAprobacion": "..." }
        [HttpPost("procesar-solicitud")]
        public async Task<IActionResult> ProcesarSolicitud([FromBody] AprobarRechazarSolicitudDTO dto)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized("No tienes permiso para realizar esta acción.");

            var resultado = await _servicio.ProcesarSolicitud(dto);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });

            if (resultado.Valor == null)
                return Ok(new { mensaje = "Solicitud rechazada correctamente." });

            return Ok(resultado.Valor);
        }

        // PATCH /api/prestamos-equipo/5/marcar-devuelto
        // Body: { "fechaEntrega": "2025-02-01T10:00:00" }
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

        // ─── Extensiones ──────────────────────────────────────────────────────────

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
        // Body: { "fechaExtensionSolicitada": "2025-02-10T00:00:00", "motivo": "..." }
        [HttpPost("{idPrestamo}/extensiones")]
        public async Task<IActionResult> SolicitarExtension(int idPrestamo, [FromBody] CrearExtensionDTO dto)
        {
            var resultado = await _servicio.SolicitarExtension(idPrestamo, dto);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });
            return Ok(resultado.Valor);
        }

        // PATCH /api/prestamos-equipo/extensiones/3/aprobar-rechazar
        // Body: { "idUsuarioAprobador": 1, "aprobado": true, "comentarioAprobacion": "..." }
        [HttpPatch("extensiones/{idExtension}/aprobar-rechazar")]
        public async Task<IActionResult> AprobarRechazarExtension(int idExtension, [FromBody] AprobarRechazarExtensionDTO dto)
        {
            if (!User.TieneRol("1", "2"))
                return Unauthorized("No tienes permiso para realizar esta acción.");

            var resultado = await _servicio.AprobarRechazarExtension(idExtension, dto);
            if (!resultado.esExitoso)
                return BadRequest(new { error = resultado.MensajeError });
            return Ok(resultado.Valor);
        }
    }
}
