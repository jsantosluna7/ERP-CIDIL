using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.Modelos;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioResetPassword : IServicioResetPassword
    {
        private readonly IRepositorioResetPassword _repositorioResetPassword;

        public ServicioResetPassword(IRepositorioResetPassword repositorioResetPassword)
        {
            _repositorioResetPassword = repositorioResetPassword;
        }

        //Método para obtener el token de restablecimiento de contraseña, y revisar si el usuario existe
        public async Task<bool?> OlvideContrasena(OlvideContrasena olvideContrasena)
        {
            var resultado = await _repositorioResetPassword.OlvideContrasena(olvideContrasena);
            return resultado;
        }

        //Método para restablecer la contraseña
        public async Task<bool?> RestablecerContrasena(ResetearContrasena resetearContrasena)
        {
            var resultado = await _repositorioResetPassword.RestablecerContrasena(resetearContrasena);
            return resultado;
        }
    }
}
