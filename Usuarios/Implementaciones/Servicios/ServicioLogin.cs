using ERP.Data.Modelos;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.LoginDTO;
using Usuarios.DTO.UsuarioDTO;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioLogin : IServicioLogin
    {
        private readonly IRepositorioLogin _repositorioLogin;

        public ServicioLogin(IRepositorioLogin repositorioLogin)
        {
            _repositorioLogin = repositorioLogin;
        }

        //El servicio Login se encarga de realizar la lógica de negocio y de interactuar con el repositorio para obtener los datos necesarios.

        //Método para iniciar seción
        public async Task<Resultado<Token?>> IniciarSecion(Login login)
        {
            var resultado = await _repositorioLogin.IniciarSecion(login);

            if (!resultado.esExitoso)
            {
                return Resultado<Token?>.Falla(resultado.MensajeError ?? "Error al registrar el usuario.");
            }

            var usuario = resultado.Valor!;

            //Aquí se retorna el token
            return Resultado<Token?>.Exito(usuario);
        }

        //Método para completar registro con OTP
        public async Task<Resultado<Token?>> verificarOtp(VerificarOtpDTO verificarOtp)
        {
            var resultado = await _repositorioLogin.verificarOtp(verificarOtp);
            if (!resultado.esExitoso)
            {
                return Resultado<Token?>.Falla(resultado.MensajeError ?? "Error al verificar el OTP.");
            }
            var usuario = resultado.Valor!;
            return Resultado<Token?>.Exito(usuario);
        }


        //Método para registrar un usuario
        public async Task<Resultado<Token?>> RegistrarUsuario(CrearRegistroDTO crearRegistroDTO) //El task es para que sea asincrono, y es de tipo CrearLoginDTO, esto es para que retorne el DTO de la clase CrearLoginDTO y que el usuario pueda ver el resultado de la creación del usuario que es ese DTO.
        {
            var resultado = await _repositorioLogin.RegistrarUsuario(crearRegistroDTO);

            if (!resultado.esExitoso)
            {
                return Resultado<Token?>.Falla(resultado.MensajeError ?? "Error al registrar el usuario.");
            }

            var usuario = resultado.Valor!;

            return Resultado<Token?>.Exito(usuario);
        }

    }
}
