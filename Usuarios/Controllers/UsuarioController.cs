using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.UsuarioDTO;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // Inyección de dependencias del servicio de usuarios
        private readonly IServicioUsuarios _servicioUsuarios;

        // Constructor que recibe el servicio de usuarios
        public UsuarioController(IServicioUsuarios servicioUsuarios)
        {
            _servicioUsuarios = servicioUsuarios;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            // Llamar al servicio para obtener la lista de usuarios
            var usuarios = await _servicioUsuarios.ObtenerUsuarios();

            // Verificar si la lista de usuarios está vacía
            if (usuarios == null)
            {
                return NotFound("Lista de de usuarios vacía");
            }
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            // Llamar al servicio para obtener un usuario por su ID
            var usuario = await _servicioUsuarios.ObtenerUsuarioPorId(id);

            // Verificar si el usuario existe
            if (usuario == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }
            return Ok(usuario);
        }

        //[HttpPost]
        //public IActionResult CrearUsuario([FromBody] UsuarioDTO usuarioDTO)
        //{
        //    // Llamar al servicio para crear un nuevo usuario
        //    var nuevoUsuario = _servicioUsuarios.crearUsuario(usuarioDTO);
        //    return Ok(nuevoUsuario);
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            // Llamar al servicio para actualizar un usuario existente
            var usuarioActualizado = await _servicioUsuarios.actualizarUsuario(id, actualizarUsuarioDTO);

            // Verificar si el usuario fue actualizado
            if (usuarioActualizado == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }
            return Ok(usuarioActualizado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            // Llamar al servicio para eliminar un usuario por su ID
            var usuario = await _servicioUsuarios.eliminarUsuario(id);

            // Verificar si el usuario fue eliminado
            if (usuario == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }

            // Devolver una respuesta exitosa
            return Ok($"Usuario con ID {id} eliminado");
        }
    }
}
