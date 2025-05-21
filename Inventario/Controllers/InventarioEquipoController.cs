using Inventario.Abstraccion.Servicios;
using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Implementaciones.Servicios;
using Inventario.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioEquipoController : ControllerBase
    {
        //Hacemos una inyeccion
        private readonly IServicioInventarioEquipo _servicioInventarioEquipo;
        private readonly DbErpContext _context;

        public InventarioEquipoController(IServicioInventarioEquipo servicioInventarioEquipo, DbErpContext context)
        {
            _servicioInventarioEquipo = servicioInventarioEquipo;
            _context = context;
        }

        //Controlador para  optener el inventario del los equipos
        [HttpGet]
        public async Task<IActionResult?> GetInventarioEquipo([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            var resultado =await _servicioInventarioEquipo.GetInventarioEquipo(pagina, tamanoPagina);
            if (resultado == null)
            {
                return NotFound("Lista de Inventario de Equipos no encontrada");
            }

            var totalInventario = await _context.InventarioEquipos.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalInventario / (double)tamanoPagina);

            var respuesta = new
            {
                paginacion = new
                {
                    paginaActual = pagina,
                    tamanoPagina,
                    totalInventario,
                    totalPaginas
                },
                datos = resultado
            };
            return Ok(respuesta);
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
