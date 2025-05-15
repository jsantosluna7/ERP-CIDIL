using IoT.Abstraccion.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        //Inyeccion de dependencia 
        private readonly IServicioIoT _ioT;

        public IoTController(IServicioIoT ioT)
        {
            _ioT = ioT;
        }

        //Controlador para optener todos los registros

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
        //Controlador para optener todos los registros por ID
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
        //Controlador para Eliminar el registro por ID

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _ioT.Eliminar(id);
            return Ok();
        }

        //Controlador para desactivar el registro por ID
        [HttpPatch("{id}")]
        public async Task<IActionResult> desactivarIoT(int id)
        {
            // Llamar al servicio para desactivar un IoT por su ID
            var IoTDesactivado = await _ioT.desactivarIoT(id);
            // Verificar si el IoT fue desactivado
            if (IoTDesactivado == null)
            {
                return NotFound($"IoT con ID {id} no encontrado");
            }
            // Devolver una respuesta exitosa
            return Ok($"IoT con ID {id} desactivado");
        }
    }
}
