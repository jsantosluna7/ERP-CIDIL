using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOPrestamosEquipo;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosEquipoController : ControllerBase
    {
        private readonly IServicioPrestamosEquipo _prestamosEquipo;

        public PrestamosEquipoController(IServicioPrestamosEquipo prestamosEquipo)
        {
            this._prestamosEquipo = prestamosEquipo;
        }


        [HttpGet]
        public async Task<IActionResult?> GetPrestamosEquipo()
        {
            var resultado = await _prestamosEquipo.GetPrestamosEquipo();
            if (resultado == null)
            {
                return NotFound("Lista no encontrada");
            }
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult?> Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO)
        {
            var resultado = await _prestamosEquipo.Crear(crearPrestamosEquipoDTO);
            if (resultado == null)
            {
                return NotFound("Lista no encontrada");
            }
            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult?> Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO)
        {
            var resultado = await _prestamosEquipo.Actualizar(id, actualizarPrestamosEquipoDTO);
            if (resultado == null)
            {
                return NotFound("Lista no encontrada");
            }
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult?> GetById(int id)
        {
            var resultado = await _prestamosEquipo.GetById(id);
            if (resultado == null)
            {
                return NotFound("Lista no encontrada");
            }
            return Ok(resultado);
            
        }

        [HttpDelete("{id}")]
        public async  Task<IActionResult?> DeleteById(int id)
        {
           await _prestamosEquipo.Eliminar(id);
            return Ok();
        }
    }
}
