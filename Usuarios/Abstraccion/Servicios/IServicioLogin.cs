using Usuarios.DTO.LoginDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioLogin
    {
        Task<LoginDTO?> IniciarSecion(Login login);
        Task<CrearRegistroDTO?> RegistrarUsuario(CrearRegistroDTO crearLoginDTO);
    }
}