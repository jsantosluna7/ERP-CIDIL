using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioUsuarios
    {
        UsuarioDTO actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO);
        UsuarioDTO crearUsuario(UsuarioDTO usuarioDTO);
        void eliminarUsuario(int id);
        Usuario ObtenerUsuarioPorId(int id);
        List<UsuarioDTO> ObtenerUsuarios();
    }
}