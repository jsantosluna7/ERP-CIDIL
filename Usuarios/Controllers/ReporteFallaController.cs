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
            if (resultado == null)
            {
                return NotFound("No se pudo encontrar los Reportes");
            }
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CrearReporte(CrearReporteFallaDTO crearReporteFallaDTO)
        {
            var resultado = await _servicioReporteFalla.CrearReporte(crearReporteFallaDTO);
            if (resultado == null)
            {
                return NotFound("No se pudo crear el reporte");
            }
            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarReporte(int id, ActualizarReporteFallaDTO actualizarReporteFallaDTO)
        {
            var resultado = await _servicioReporteFalla.ActualizarReporte(id, actualizarReporteFallaDTO);
            if(resultado == null)
            {
                return NotFound("No se pudo actualizar el reporte");
            }
            return Ok(resultado);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdReporte(int id)
        {
            var resultado = await _servicioReporteFalla.GetByIdReporteFalla(id);
            if(resultado == null)
            {
                return NotFound("No se pudo encontrar el reporte con ese Id");
            }
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _servicioReporteFalla.Eliminar(id);
            return Ok();
        }
    }
}
