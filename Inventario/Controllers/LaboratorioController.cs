using ERP.Data.Modelos;
using Inventario.Abstraccion.Servicios;
using Inventario.DTO.LaboratorioDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratorioController : ControllerBase
    {

        //Hacemos una inyeccion
        private readonly IServicioLaboratorio _servicioLaboratorio;
        private readonly DbErpContext _context;

        public LaboratorioController(IServicioLaboratorio servicioLaboratorio, DbErpContext context)
        {
            _servicioLaboratorio = servicioLaboratorio;
            _context = context;
        }

        //Controlador para  optener el inventario del los Laboratorios
        [HttpGet]
        public async Task<IActionResult?> GetLaboratorio() 
        {
            var resultado =await _servicioLaboratorio.GetLaboratorio();
            if (resultado == null)
            {
                return NotFound("No se Pudo Encontrar la Lista de Laboratorios");
            }
            return Ok(resultado);
        }
        //Controlador para  incertar los equipos en el inventario 
        [HttpPost]
        public async Task<IActionResult?> Crear(CrearLaboratorioDTO crearLaboratorioDTO)
        {
            var resultado =await _servicioLaboratorio.Crear(crearLaboratorioDTO);
            if (resultado == null)
            {
                return NotFound("No se pudo Crear la lista de Laboratorio");
            }
            return Ok(resultado);
        }

        //Controlador para  Actualizar el inventario del los equipos
        [HttpPut("{id}")]
        public async Task<IActionResult?> Actualizar(int id,ActualizarLaboratorioDTO actualizarLaboratorioDTO)
        {
            var resultado =await _servicioLaboratorio.Actualizar(id, actualizarLaboratorioDTO);
            if (resultado == null)
            {
                return NotFound("No se pudo Actualizar la lista de Laboratorios");
            }
            return Ok(resultado);
        }

        //Controlador para  optener el inventario del los equipos por ID

        [HttpGet("{id}")]
        public async Task<IActionResult?> GetByid(int id)
        {
            var resultado =await _servicioLaboratorio.GetById(id);
            if (resultado == null)
            {
                return NotFound($"No se pudo encontrar el Laboratorio con el ID:{id}");
            }
            return Ok(resultado);
        }

        [HttpGet("laboratorios")]
        public async Task<IActionResult> GetCodigoLab(string codigo)
        {
            var lab = await _servicioLaboratorio.obtenerPorCodigo(codigo);
            if (lab == null) return NotFound();
            return Ok(lab);
        }

        [HttpGet("pisos")]
        public async Task<IActionResult?> GetPisos([FromQuery] int piso = 1)
        {
            var resultado = await _servicioLaboratorio.GetPisos(piso);
            if (resultado == null)
            {
                return NotFound("El piso no existe o no fue encontrado");
            }
            return Ok(resultado);
        }

        //Controlador para Borrar los Laboratorios por ID

        [HttpDelete("{id}")]
        public async Task<IActionResult?> Eliminar(int id)
        {
          await  _servicioLaboratorio.Eliminar(id);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult?> DesactivarLaboratorio(int id)
        {
           var laboratorioAct = await _servicioLaboratorio.DesactivarLaboratorio(id);
            if(laboratorioAct == null)
            {
                return NotFound($"No se pudo encontrar el Laboratorio con el ID:{id}");
            }
            return Ok();
        }


    }
}
