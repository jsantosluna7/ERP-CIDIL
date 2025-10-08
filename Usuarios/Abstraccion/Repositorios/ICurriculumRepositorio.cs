using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    /// <summary>
    /// Interfaz para manejar la persistencia de Curriculum
    /// </summary>
    public interface ICurriculumRepositorio
    {
        /// <summary>
        /// Obtiene todos los curriculums
        /// </summary>
        Task<List<Curriculum>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un curriculum por Id
        /// </summary>
        Task<Curriculum?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo curriculum
        /// </summary>
        Task CrearAsync(Curriculum curriculum);

        /// <summary>
        /// Actualiza un curriculum existente
        /// </summary>
        Task ActualizarAsync(Curriculum curriculum);

        /// <summary>
        /// Elimina un curriculum por Id
        /// </summary>
        Task<bool> EliminarAsync(int id);

        /// <summary>
        /// Guarda los cambios pendientes en la base de datos
        /// </summary>
        Task GuardarAsync();
    }
}
