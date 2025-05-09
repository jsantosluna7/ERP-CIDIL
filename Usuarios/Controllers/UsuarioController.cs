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
        public IActionResult ObtenerUsuarios()
        {
            // Llamar al servicio para obtener la lista de usuarios
            var usuarios = _servicioUsuarios.ObtenerUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerUsuarioPorId(int id)
        {
            // Llamar al servicio para obtener un usuario por su ID
            var usuario = _servicioUsuarios.ObtenerUsuarioPorId(id);
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
        public IActionResult ActualizarUsuario(int id, [FromBody] ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            // Llamar al servicio para actualizar un usuario existente
            var usuarioActualizado = _servicioUsuarios.actualizarUsuario(id, actualizarUsuarioDTO);
            return Ok(usuarioActualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            // Llamar al servicio para eliminar un usuario por su ID
            _servicioUsuarios.eliminarUsuario(id);
            return Ok();
        }
    }
}
