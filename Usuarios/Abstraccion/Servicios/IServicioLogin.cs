using ERP.Data.Modelos;
using Usuarios.DTO.LoginDTO;
using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioLogin
    {
        Task<Resultado<GoogleAuthResultDTO?>> AutenticarConGoogle(GoogleAuthDTO googleAuthDto);
        Task<Resultado<Token?>> CompletarRegistroGoogle(CompletarRegistroGoogleDTO dto);
        Task<Resultado<Token?>> IniciarSecion(Login login);
        Task<Resultado<Token?>> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO);
        Task<Resultado<Token?>> verificarOtp(VerificarOtpDTO verificarOtp);
    }
}