using Usuarios.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioResetPassword
    {
        Task<bool?> OlvideContrasena(OlvideContrasena olvideContrasena);
        Task<bool?> RestablecerContrasena(ResetearContrasena resetearContrasena);
    }
}