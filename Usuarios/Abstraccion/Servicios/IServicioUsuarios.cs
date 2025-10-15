using ERP.Data.Modelos;
using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioUsuarios
    {
        Task<ActualizarUsuarioDTO?> actualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO);
        Task<Resultado<List<Usuario>>> BuscarUsuario(string termino, string filtro);
        Task<bool?> desactivarUsuario(int id);
        Task<bool?> eliminarUsuario(int id);
        Task<Usuario?> ObtenerUsuarioPorId(int id);

        //UsuarioDTO crearUsuario(UsuarioDTO usuarioDTO);
        //Task<List<UsuarioDTO>?> ObtenerUsuarios();
        Task<List<UsuarioDTO>?> ObtenerUsuarios(int pagina, int tamanoPagina);
        Task<List<UsuarioDTO>?> ObtenerUsuariosTodos();
    }
}