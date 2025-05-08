using Inventario.Abstraccion.Servicios;
using Inventario.DTO.InventarioEquipoDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioEquipoController : ControllerBase
    {
        //Hacemos una inyeccion
        private readonly IServicioInventarioEquipo _servicioInventarioEquipo;

        public InventarioEquipoController(IServicioInventarioEquipo servicioInventarioEquipo)
        {
            _servicioInventarioEquipo = servicioInventarioEquipo;
        }

        //Controlador para  optener el inventario del los equipos
        [HttpGet]
        public IActionResult GetInventarioEquipo()
        {
            var resultado = _servicioInventarioEquipo.GetInventarioEquipo();
            return Ok(resultado);
        }
        //Controlador para  incertar  los equipos
        [HttpPost]
        public IActionResult CrearInventarioEquipo(CrearInventarioEquipoDTO crearInventarioEquipoDTO)
        {
            var resultado =_servicioInventarioEquipo.Crear(crearInventarioEquipoDTO);
            return Ok(resultado);
        }

        //Controlador para  Actualizar el inventario del los equipos
        [HttpPut("{id}")]

        public IActionResult Actualizar(int id,ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO) 
        {
            var resultado = _servicioInventarioEquipo.Actualizar(id, actualizarInventarioEquipoDTO);
            return Ok(resultado);
        }

        //Controlador para  optener por ID el inventario del los equipos
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        { 
            var resultado= _servicioInventarioEquipo.GetById(id);
            return Ok(resultado);
        }

        //Controlador para  Eliminar el inventario del los equipos por ID
        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id) 
        { 
            _servicioInventarioEquipo.Eliminar(id);
            return Ok();
        }

    }
}
