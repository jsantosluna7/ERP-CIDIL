using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    /// <summary>
    /// Interfaz para manejar la persistencia de Comentarios
    /// </summary>
    public interface IComentarioRepositorio
    {
        /// <summary>
        /// Obtiene todos los comentarios de la base de datos.
        /// </summary>
        Task<List<Comentario>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un comentario específico por su Id.
        /// </summary>
        /// <param name="id">Id del comentario</param>
        Task<Comentario?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo comentario en la base de datos.
        /// </summary>
        /// <param name="comentario">Objeto Comentario a crear</param>
        Task CrearAsync(Comentario comentario);

        /// <summary>
        /// Actualiza un comentario existente de manera asincrónica
        /// </summary>
        /// <param name="comentario">Objeto Comentario con los cambios</param>
        Task ActualizarAsync(Comentario comentario);

        /// <summary>
        /// Elimina un comentario existente de manera asincrónica
        /// </summary>
        /// <param name="comentario">Objeto Comentario a eliminar</param>
        Task EliminarAsync(Comentario comentario);

        /// <summary>
        /// Guarda los cambios pendientes en la base de datos.
        /// </summary>
        Task GuardarAsync();

        /// <summary>
        /// Elimina un comentario por Id de manera asincrónica.
        /// </summary>
        /// <param name="id">Id del comentario a eliminar</param>
        /// <returns>True si se eliminó, false si no se encontró</returns>
        Task<bool> EliminarPorIdAsync(int id);
    }
}
