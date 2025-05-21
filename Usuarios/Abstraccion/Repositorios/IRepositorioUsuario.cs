using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioUsuario
    {
        Task<Usuario?> actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO);
        Task<bool?> desactivarUsuario(int id);

        //Usuario crearUsuario(UsuarioDTO usuarioDTO);
        Task<bool?> eliminarUsuario(int id);
        Task<Usuario?> obtenerUsuarioPorId(int id);
        //Task<List<Usuario>> obtenerUsuarios();
        Task<List<Usuario>> obtenerUsuarios(int pagina, int tamanoPagina);
    }
}