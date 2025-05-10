using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioUsuarios
    {
        Task<UsuarioDTO?> actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO);
        Task<bool?> eliminarUsuario(int id);
        Task<Usuario?> ObtenerUsuarioPorId(int id);

        //UsuarioDTO crearUsuario(UsuarioDTO usuarioDTO);
        Task<List<UsuarioDTO>?> ObtenerUsuarios();
    }
}