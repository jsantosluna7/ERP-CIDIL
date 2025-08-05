using ERP.Data.Modelos;
using Usuarios.DTO.LoginDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioLogin
    {
        Task<Resultado<Usuario?>> IniciarSecion(Login login);
        Task<Resultado<UsuariosPendiente?>> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO);
        Task<Resultado<Usuario?>> verificarOtp(VerificarOtpDTO verificarOtp);
    }
}