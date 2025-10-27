using ERP.Data.Modelos;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    /// <summary>
    /// Interfaz para manejar usuarios institucionales.
    /// Proporciona métodos para obtener usuarios por ID o correo,
    /// y para guardar cambios en la base de datos.
    /// </summary>
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtener un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario o null si no existe</returns>
        Task<Usuario?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Obtener un usuario por su correo institucional.
        /// </summary>
        /// <param name="correo">Correo institucional del usuario</param>
        /// <returns>Usuario o null si no existe</returns>
        Task<Usuario?> ObtenerPorCorreoAsync(string correo);

        /// <summary>
        /// Guardar cambios pendientes en la base de datos.
        /// </summary>
        Task GuardarAsync();
    }
}

