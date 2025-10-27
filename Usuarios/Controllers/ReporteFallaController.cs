using ERP.Data.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.ReporteFallaDTO;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteFallaController : ControllerBase
    {
        private readonly IServicioReporteFalla _servicioReporteFalla;
        private readonly DbErpContext _context;

        public ReporteFallaController(IServicioReporteFalla servicioReporteFalla, DbErpContext context)
        {
            _servicioReporteFalla = servicioReporteFalla;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult?> GetReporteFalla()
        {
            var resultado = await _servicioReporteFalla.GetReporteFalla();
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpGet("mis-reportes")]
        public async Task<IActionResult> ObtenerMisReportes([FromQuery] int usuario)
        {
            var resultado = await _servicioReporteFalla.ObtenerReporteFallaUsuario(usuario);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPost]
        public async Task<IActionResult> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO)
        {
            var resultado = await _servicioReporteFalla.CrearReporte(crearReporteFallaDTO);
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO)
        {
            var resultado = await _servicioReporteFalla.ActualizarReporte(id, actualizarReporteFallaDTO);
            if(!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdReporte(int id)
        {
            var resultado = await _servicioReporteFalla.GetByIdReporteFalla(id);
            if(!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resultado = await _servicioReporteFalla.Eliminar(id);
            if(!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }
    }
}
