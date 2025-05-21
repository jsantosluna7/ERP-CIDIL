using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // Inyección de dependencias del servicio de usuarios
        private readonly IServicioUsuarios _servicioUsuarios;
        private readonly DbErpContext _context;

        // Constructor que recibe el servicio de usuarios
        public UsuarioController(IServicioUsuarios servicioUsuarios, DbErpContext context)
        {
            _servicioUsuarios = servicioUsuarios;
            _context = context;

        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            // Llamar al servicio para obtener la lista de usuarios
            var usuarios = await _servicioUsuarios.ObtenerUsuarios(pagina, tamanoPagina);

            // Verificar si la lista de usuarios está vacía
            if (usuarios == null)
            {
                return NotFound("Lista de de usuarios vacía");
            }

            var totalUsuarios = await _context.InventarioEquipos.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalUsuarios/ (double)tamanoPagina);

            var respuesta = new
            {
                paginacion = new
                {
                    paginaActual = pagina,
                    tamanoPagina,
                    totalUsuarios,
                    totalPaginas
                },
                datos = usuarios
            };

            return Ok(respuesta);
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> DesactivarUsuario(int id)
        {
            // Llamar al servicio para desactivar un usuario por su ID
            var usuarioDesactivado = await _servicioUsuarios.desactivarUsuario(id);
            // Verificar si el usuario fue desactivado
            if (usuarioDesactivado == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }
            // Devolver una respuesta exitosa
            return Ok($"Usuario con ID {id} desactivado");
        }
    }
}
