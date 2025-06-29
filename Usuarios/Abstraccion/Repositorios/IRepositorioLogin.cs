using ERP.Data.Modelos;
using Usuarios.DTO.LoginDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioLogin
    {
        Task<Resultado<Usuario?>> IniciarSecion(Login login);
        Task<Resultado<Usuario?>> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO);
    }
}