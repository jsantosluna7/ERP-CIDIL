using Inventario.Abstraccion.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoFisicoController : ControllerBase
    {
        private readonly IServicioEstadoFisico _estadoFisico;

        public EstadoFisicoController(IServicioEstadoFisico estadoFisico)
        {
            _estadoFisico = estadoFisico;
        }

        [HttpGet]
        public async Task<IActionResult?> GetEstadoFisico()
        {
            var resultado = await _estadoFisico.GetEstadoFisico();
            if (resultado == null)
            {
                return NotFound("Lista de estado no encontrada");
            }
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult?> GetById(int id)
        {
            var resultado = await _estadoFisico.GetById(id);
            if (resultado == null)
            {
                return NotFound("Lista de estado no encontrada");
            }
            return Ok(resultado);
        }
    }
}
