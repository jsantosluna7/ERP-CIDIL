using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioUsuarios
    {
        Task<ActualizarUsuarioDTO?> actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO);
        Task<bool?> desactivarUsuario(int id);
        Task<bool?> eliminarUsuario(int id);
        Task<Usuario?> ObtenerUsuarioPorId(int id);

        //UsuarioDTO crearUsuario(UsuarioDTO usuarioDTO);
        Task<List<UsuarioDTO>?> ObtenerUsuarios();
    }
}