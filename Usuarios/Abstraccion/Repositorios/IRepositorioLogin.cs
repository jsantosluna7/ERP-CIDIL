using ERP.Data.Modelos;
using Usuarios.DTO.LoginDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioLogin
    {
        Task<Usuario?> IniciarSecion(Login login);
        Task<Usuario?> RegistrarUsuario(CrearRegistroDTO crearLoginDTO);
    }
}