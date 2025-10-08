using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios; // ✅ Aquí están ComentarioDTO, CrearComentarioDTO, ActualizarComentarioDTO y ComentarioDetalleDTO

namespace Usuarios.Abstraccion.Servicios
{
    /// <summary>
    /// Interfaz que define los métodos del servicio de Comentarios
    /// </summary>
    public interface IComentarioServicio
    {
        /// <summary>
        /// Obtiene todos los comentarios con detalles.
        /// </summary>
        Task<List<ComentarioDetalleDTO>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un comentario específico por su Id.
        /// </summary>
        /// <param name="id">Id del comentario</param>
        Task<ComentarioDetalleDTO?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo comentario usando un DTO.
        /// </summary>
        /// <param name="dto">Datos para crear un comentario</param>
        Task CrearAsync(CrearComentarioDTO dto);

        /// <summary>
        /// Actualiza un comentario existente usando un DTO.
        /// </summary>
        /// <param name="id">Id del comentario a actualizar</param>
        /// <param name="dto">Datos para actualizar el comentario</param>
        /// <returns>True si se actualizó, false si no se encontró</returns>
        Task<bool> ActualizarAsync(int id, ActualizarComentarioDTO dto);

        /// <summary>
        /// Elimina un comentario por Id.
        /// </summary>
        /// <param name="id">Id del comentario a eliminar</param>
        /// <returns>True si se eliminó, false si no se encontró</returns>
        Task<bool> EliminarAsync(int id);
    }
}
