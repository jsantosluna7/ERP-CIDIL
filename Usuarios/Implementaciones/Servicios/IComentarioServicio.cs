using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Abstraccion.Servicios
{
    /// <summary>
    /// Define los métodos de negocio para la gestión de comentarios.
    /// </summary>
    public interface IComentarioServicio
    {
        /// <summary>
        /// Obtiene todos los comentarios del sistema, incluyendo el nombre del usuario y el título del anuncio.
        /// </summary>
        /// <returns>Lista de comentarios detallados.</returns>
        Task<List<ComentarioDetalleDTO>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un comentario específico por su ID.
        /// </summary>
        /// <param name="id">ID del comentario.</param>
        /// <returns>Comentario detallado o null si no existe.</returns>
        Task<ComentarioDetalleDTO?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Obtiene todos los comentarios asociados a un anuncio específico.
        /// </summary>
        /// <param name="anuncioId">ID del anuncio.</param>
        /// <returns>Lista de comentarios relacionados con el anuncio.</returns>
        Task<List<ComentarioDetalleDTO>> ObtenerPorAnuncioIdAsync(int anuncioId);

        /// <summary>
        /// Crea un nuevo comentario asociado a un anuncio y usuario.
        /// </summary>
        /// <param name="dto">Datos del comentario a crear.</param>
        /// <returns>El comentario creado, incluyendo los datos del usuario y anuncio.</returns>
        Task<ComentarioDetalleDTO> CrearAsync(CrearComentarioDTO dto);

        /// <summary>
        /// Actualiza el texto de un comentario existente.
        /// </summary>
        /// <param name="id">ID del comentario a actualizar.</param>
        /// <param name="dto">Datos actualizados.</param>
        /// <returns>True si se actualizó correctamente, false si no existe o no es válido.</returns>
        Task<bool> ActualizarAsync(int id, ActualizarComentarioDTO dto);

        /// <summary>
        /// Elimina un comentario por su ID.
        /// </summary>
        /// <param name="id">ID del comentario.</param>
        /// <returns>True si se eliminó correctamente, false si no se encontró.</returns>
        Task<bool> EliminarAsync(int id);
    }
}
