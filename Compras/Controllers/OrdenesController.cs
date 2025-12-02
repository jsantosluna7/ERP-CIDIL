using Compras.Abstraccion.Servicios;
using Compras.Implementaciones.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Compras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenesController : ControllerBase
    {
        private readonly IServicioOrdenes _servicioOrdenes;
        public OrdenesController(IServicioOrdenes servicioOrdenes)
        {
            _servicioOrdenes = servicioOrdenes;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerOrdenes()
        {
            //Aquí puedes llamar al servicio para registrar el usuario
            var resultado = await _servicioOrdenes.OrdenesAll();
            if (!resultado.esExitoso)
            {
                return BadRequest(new { error = resultado.MensajeError });
            }
            return Ok(resultado.Valor);
        }
    }
}
