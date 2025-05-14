using IoT.Abstraccion.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        private readonly IServicioIoT _ioT;

        public IoTController(IServicioIoT ioT)
        {
            _ioT = ioT;
        }

        [HttpGet]
        public async Task<IActionResult> GetIot()
        {
            var resultado = await _ioT.GetIot();
            if (resultado == null)
            {
                return NotFound("Lista de Informacion de  no encontrada");
            }
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdIot(int id)
        {

            var resultado = await _ioT.GetByIdIoT(id);
            if (resultado == null)
            {
                return NotFound("No se pudo Encontrar el inventario ");
            }
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _ioT.Eliminar(id);
            return Ok();
        }
    }
}
