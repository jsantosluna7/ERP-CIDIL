using ERP.Data.Modelos;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    // Interfaz para manejar usuarios institucionales. Proporciona métodos para obtener usuarios por ID o correo, y para guardar cambios en la base de datos.
    public interface IUsuarioRepositorio
    {
        // Obtener un usuario por su ID.
        Task<Usuario?> ObtenerPorIdAsync(int id);

        // Obtener un usuario por su correo institucional.
        Task<Usuario?> ObtenerPorCorreoAsync(string correo);

        // Guardar cambios pendientes en la base de datos.
        Task GuardarAsync();
    }
}
