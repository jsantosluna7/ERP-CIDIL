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
        public IActionResult GetPrestamosEquipo()
        {
            var resultado = _prestamosEquipo.GetPrestamosEquipo();
            return Ok(resultado);
        }

        [HttpPost]
        public IActionResult Crear(CrearPrestamosEquipoDTO crearPrestamosEquipoDTO)
        {
            var resultado = _prestamosEquipo.Crear(crearPrestamosEquipoDTO);
            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, ActualizarPrestamosEquipoDTO actualizarPrestamosEquipoDTO)
        {
            var resultado = _prestamosEquipo.Actualizar(id, actualizarPrestamosEquipoDTO);
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var resultado = _prestamosEquipo.GetById(id);
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            _prestamosEquipo.Eliminar(id);
            return Ok();
        }
    }
}
