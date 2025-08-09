using ERP.Data.Modelos;
using Usuarios.DTO.LoginDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioLogin
    {
        Task<Resultado<Token?>> IniciarSecion(Login login);
        Task<Resultado<Token?>> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO);
        Task<Resultado<Token?>> verificarOtp(VerificarOtpDTO verificarOtp);
    }
}