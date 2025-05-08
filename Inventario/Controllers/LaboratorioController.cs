using Inventario.Abstraccion.Servicios;
using Inventario.DTO.LaboratorioDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratorioController : ControllerBase
    {

        //Hacemos una inyeccion
        private readonly IServicioLaboratorio _servicioLaboratorio;

        public LaboratorioController(IServicioLaboratorio servicioLaboratorio)
        {
            _servicioLaboratorio = servicioLaboratorio;
        }

        //Controlador para  optener el inventario del los equipos
        [HttpGet]
        public IActionResult GetLaboratorio() 
        {
            var resultado = _servicioLaboratorio.GetLaboratorio();
            return Ok(resultado);
        }
        //Controlador para  incertar los equipos en el inventario 
        [HttpPost]
        public IActionResult Crear(CrearLaboratorioDTO crearLaboratorioDTO)
        {
            var resultado = _servicioLaboratorio.Crear(crearLaboratorioDTO);
            return Ok(resultado);
        }

        //Controlador para  Actualizar el inventario del los equipos
        [HttpPut("{id}")]
        public IActionResult Actualizar(int id,ActualizarLaboratorioDTO actualizarLaboratorioDTO)
        {
            var resultado = _servicioLaboratorio.Actualizar(id, actualizarLaboratorioDTO);
            return Ok(resultado);
        }

        //Controlador para  optener el inventario del los equipos por ID

        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            var resultado = _servicioLaboratorio.GetById(id);
            return Ok(resultado);
        }

        //Controlador para Borrar los equipos por ID

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            _servicioLaboratorio.Eliminar(id);
            return Ok();
        }
    }
}
