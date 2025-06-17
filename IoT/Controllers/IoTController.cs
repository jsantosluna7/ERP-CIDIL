using ERP.Data.Modelos;
using IoT.Abstraccion.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IoTController : ControllerBase
    {
        //Inyeccion de dependencia 
        private readonly IServicioIoT _ioT;
        private readonly DbErpContext _context;

        public IoTController(IServicioIoT ioT, DbErpContext context)
        {
            _ioT = ioT;
            _context = context;
        }

        //Controlador para optener todos los registros

        [HttpGet]
        public async Task<IActionResult> GetIot([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            var resultado = await _ioT.GetIot(pagina, tamanoPagina);

            if (resultado == null)
            {
                return NotFound("Lista de Informacion de  no encontrada");
            }

            var totalLoT = await _context.Iots.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalLoT / (double)tamanoPagina);

            var respuesta = new
            {
                paginacion = new
                {
                    paginaActual = pagina,
                    tamanoPagina,
                    totalLoT,
                    totalPaginas
                },
                datos = resultado
            };

            return Ok(respuesta);
        }

        [HttpGet("filtro-fecha")]
        public async Task<IActionResult> filtroFecha([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, [FromQuery] DateTime inicio = default(DateTime), [FromQuery] DateTime fin = default(DateTime))
        {
            var resultado = await _ioT.filtroFecha(pagina, tamanoPagina, inicio, fin);

            if (resultado == null)
            {
                return NotFound("Lista de Informacion de  no encontrada");
            }

            var totalLoT = await _context.Iots.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalLoT / (double)tamanoPagina);

            var respuesta = new
            {
                paginacion = new
                {
                    paginaActual = pagina,
                    tamanoPagina,
                    totalLoT,
                    totalPaginas
                },
                datos = resultado
            };

            return Ok(respuesta);
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
