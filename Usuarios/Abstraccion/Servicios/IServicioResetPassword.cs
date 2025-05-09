using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioResetPassword
    {
        Task<bool?> OlvideContrasena(OlvideContrasena olvideContrasena);
        Task<bool?> RestablecerContrasena(ResetearContrasena resetearContrasena);
    }
}