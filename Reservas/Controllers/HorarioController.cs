using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservas.Abstraccion.Servicios;
using Reservas.DTO.DTOHorario;

namespace Reservas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly IServicioHorario _servicioHorario;
        public HorarioController(IServicioHorario servicioHorario)
        {
            _servicioHorario = servicioHorario;
        }

        // Método para obtener todos los horarios
        [HttpGet]
        public async Task<IActionResult> ObtenerHorarios()
        {
            //Llamar al servicio para obtener todos los horarios
            var horarios = await _servicioHorario.ObtenerHorarios();

            //verificar si la lista de horarios está vacía
            if (horarios == null)
            {
                return NotFound("Lista de horarios vacía");
            }

            // Devolver la lista de horarios
            return Ok(horarios);
        }

        // Método para obtener un horario por id
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerHorarioPorId(int id)
        {
            //Llamar al servicio para obtener un horario por id
            var horario = await _servicioHorario.ObtenerHorarioPorId(id);

            //verificar si el horario existe
            if (horario == null)
            {
                return NotFound("Horario no encontrado");
            }

            // Devolver el horario encontrado
            return Ok(horario);
        }

        // Método para crear un nuevo horario
        [HttpPost]
        public async Task<IActionResult> CrearHorario([FromBody] CrearHorarioDTO crearHorarioDTO)
        {
            //Llamar al servicio para actualizar un horario existente
            var nuevoHorario = await _servicioHorario.CrearHorario(crearHorarioDTO);

            //verificar si el horario ya existe
            if (nuevoHorario == null)
            {
                return BadRequest("El horario ya existe");
            }

            // Devolver el nuevo horario creado
            return Ok(nuevoHorario);
        }

        // Método para actualizar un horario existente
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarHorario(int id, [FromBody] ActualizarHorarioDTO actualizarHorarioDTO)
        {
            //Llamar al servicio para actualizar un horario existente
            var horarioActualizado = await _servicioHorario.ActualizarHorario(id, actualizarHorarioDTO);
            //verificar si el horario fue actualizado
            if (horarioActualizado == null)
            {
                return NotFound("Horario no encontrado o no existe.");
            }
            // Devolver el horario actualizado
            return Ok(horarioActualizado);
        }

        // Método para borrar un horario existente
        [HttpDelete("{id}")]
        public async Task<IActionResult> BorrarHorario(int id)
        {
            //Llamar al servicio para borrar un horario existente
            var resultado = await _servicioHorario.BorrarHorario(id);
            //verificar si el horario fue borrado
            if (resultado == null)
            {
                return NotFound("Horario no encontrado");
            }
            // Devolver el resultado de la operación
            return Ok(resultado);
        }

        // Método para borrar un horario existente
        [HttpDelete("automatico")]
        public async Task<IActionResult> BorrarHorarioAutomatico(bool eliminar)
        {
            if (eliminar)
            {
                //Llamar al servicio para borrar un horario existente
                var resultado = await _servicioHorario.BorrarHorarioAutomatico(eliminar);
                //verificar si el horario fue borrado
                if (resultado == null)
                {
                    return NotFound("No se puede eliminar el horario porque aun no ha pasado el tiempo, si quieres eliminar un horario en especifico ve a eliminacion por id.");
                }
                else if (resultado == true)
                {    // Devolver el resultado de la operación
                    return Ok("Horarios expirados eliminados correctamente");
                }
                else if (resultado == false)
                {
                    return BadRequest("No se pudo eliminar los horarios, por un error.");
                }
                else
                {
                    return BadRequest("No se pudo eliminar los horarios, por un error.");
                }
            }
            else
            {
                return BadRequest("No se puede eliminar porque canselaste la operacion.");
            }
        }
    }
}
