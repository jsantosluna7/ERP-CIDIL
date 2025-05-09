using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioUsuario
    {
        Usuario actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO);
        //Usuario crearUsuario(UsuarioDTO usuarioDTO);
        void eliminarUsuario(int id);
        Usuario? obtenerUsuarioPorId(int id);
        List<Usuario> obtenerUsuarios();
    }
}