using ERP.Data.Modelos;
using Inventario.Abstraccion.Servicios;
using Inventario.DTO.InventarioEquipoDTO;
using Inventario.Implementaciones.Servicios;
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


        [HttpGet("ObtenerTodos")]
        public async Task<IActionResult> GetInventarioEquipo()
        {
            var resultado = await _context.InventarioEquipos
        .Select(i => new {
            i.Id,
            i.Nombre,
            i.NombreCorto,
            i.Perfil,
            i.IdLaboratorio,
            i.Fabricante,
            i.Modelo,
            i.Serial,
            i.DescripcionLarga,
            i.FechaTransaccion,
            i.Departamento,
            i.ImporteActivo,
            i.ImagenEquipo,
            i.Disponible,
            i.IdEstadoFisico,
            i.ValidacionPrestamo,
            i.Cantidad,
            i.Activado
        }).ToListAsync();

            return Ok(resultado);
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

        //Para insertar los archivos en lotes

        [HttpPost("CrearLote")]
        public async Task<IActionResult?> CrearInventarioEquipo([FromBody] List<CrearInventarioEquipoDTO> equipos)
        {
            if (equipos == null || !equipos.Any())
            {
                return BadRequest("No se enviaron los equipos");
            }
                
            foreach (var equipo in equipos)
            {
                await _servicioInventarioEquipo.Crear(equipo);
            }

            return Ok(new { mensaje = $"{equipos.Count} equipos creados correctamente." });
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
        [HttpGet("ById{id}")]
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

        [HttpPost("subir-imagen")]
        public async Task<IActionResult> SubirImagen(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("No se ha enviado ningún archivo.");

            var archivoSubida = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "inventario");
            if (!Directory.Exists(archivoSubida))
                Directory.CreateDirectory(archivoSubida);

            var nombreUnico = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
            var direccionArchivo = Path.Combine(archivoSubida, nombreUnico);

            using (var stream = new FileStream(direccionArchivo, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var relativePath = $"/inventario/{nombreUnico}"; // Esto es lo que se guarda en la BD y se envía al front
            return Ok(new { ruta = relativePath });
        }


    }
}
