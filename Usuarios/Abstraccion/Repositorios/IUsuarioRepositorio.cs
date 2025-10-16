using ERP.Data.Modelos;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    /// <summary>
    /// Interfaz mínima para manejar usuarios (puede ampliarse según tus necesidades).
    /// </summary>
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtener un usuario por su ID.
        /// </summary>
        Task<Usuario?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Guardar cambios en la base de datos.
        /// </summary>
        Task GuardarAsync();
    }
}
