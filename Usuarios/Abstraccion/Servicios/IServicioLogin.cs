using ERP.Data.Modelos;
using Usuarios.DTO.LoginDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioLogin
    {
        Task<Resultado<LoginDTO?>> IniciarSecion(Login login);
        Task<Resultado<UsuariosPendiente?>> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO);
        Task<Resultado<Usuario?>> verificarOtp(VerificarOtpDTO verificarOtp);
    }
}