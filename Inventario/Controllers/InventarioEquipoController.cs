using Inventario.Abstraccion.Servicios;
using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Implementaciones.Servicios;
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
        public async Task<IActionResult?> GetInventarioEquipo()
        {
            var resultado =await _servicioInventarioEquipo.GetInventarioEquipo();
            if (resultado == null)
            {
                return NotFound("Lista de Inventario de Equipos no encontrada");
            }
            return Ok(resultado);
        }
        //Controlador para  incertar  los equipos
        [HttpPost]
        public async Task<IActionResult?> CrearInventarioEquipo(CrearInventarioEquipoDTO crearInventarioEquipoDTO)
        {
            var resultado =await _servicioInventarioEquipo.Crear(crearInventarioEquipoDTO);
            if (resultado == null)
            {
                return NotFound("No se pudo crear el inventario de Equipos");
            }
            return Ok(resultado);
        }

        //Controlador para  Actualizar el inventario del los equipos
        [HttpPut("{id}")]

        public async Task<IActionResult?> Actualizar(int id,ActualizarInventarioEquipoDTO actualizarInventarioEquipoDTO) 
        {
            var resultado =await _servicioInventarioEquipo.Actualizar(id, actualizarInventarioEquipoDTO);
            if (resultado == null)
            {
                return NotFound("No se Pudo Actualizar el Inventario");
            }
            return Ok(resultado);
        }

        //Controlador para  optener por ID el inventario del los equipos
        [HttpGet("{id}")]
        public async Task<IActionResult?> GetById(int id)
        { 
            var resultado=await _servicioInventarioEquipo.GetById(id);
            if (resultado == null)
            {
                return NotFound("No se pudo Encontrar el inventario ");
            }
            return Ok(resultado);
        }

        //Controlador para  Eliminar el inventario del los equipos por ID
        [HttpDelete("{id}")]
        public async Task<IActionResult?> DeleteById(int id) 
        { 
           await _servicioInventarioEquipo.Eliminar(id);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult?> DesactivarEquipo(int id)
        {
            var equipo = await _servicioInventarioEquipo.DesactivarEquipo(id);
            if (equipo == null)
            {
                return NotFound($"No se pudo encontrar el Equipo con el ID:{id}");
            }
            return Ok();
        }

    }
}
