using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Abstraccion.Servicios
{
    
    /// Define los métodos de negocio para la gestión de comentarios.

    public interface IComentarioServicio
    {
      
        /// Obtiene todos los comentarios del sistema, incluyendo el nombre del usuario y el título del anuncio.
        
        /// <returns>Resultado con la lista de comentarios detallados.</returns>
        Task<Resultado<List<ComentarioDetalleDTO>>> ObtenerTodosAsync();

        
        
        Task<Resultado<ComentarioDetalleDTO>> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Obtiene todos los comentarios asociados a un anuncio específico.
        /// </summary>
        /// <param name="anuncioId">ID del anuncio.</param>
        /// <returns>Resultado con la lista de comentarios relacionados con el anuncio.</returns>
        Task<Resultado<List<ComentarioDetalleDTO>>> ObtenerPorAnuncioIdAsync(int anuncioId);

        /// <summary>
        /// Crea un nuevo comentario asociado a un anuncio y usuario.
        /// </summary>
        /// <param name="dto">Datos del comentario a crear.</param>
        /// <returns>Resultado con el comentario creado, incluyendo los datos del usuario y anuncio.</returns>
        Task<Resultado<ComentarioDetalleDTO>> CrearAsync(CrearComentarioDTO dto);

        /// <summary>
        /// Actualiza el texto de un comentario existente.
        /// </summary>
        /// <param name="id">ID del comentario a actualizar.</param>
        /// <param name="dto">Datos actualizados.</param>
        /// <returns>Resultado indicando si se actualizó correctamente.</returns>
        Task<Resultado<bool>> ActualizarAsync(int id, ActualizarComentarioDTO dto);

        
        // Elimina un comentario por su ID.
        
        /// <param name="id">ID del comentario.</param>
        /// <returns>Resultado indicando si se eliminó correctamente.</returns>
        Task<Resultado<bool>> EliminarAsync(int id);
    }
}
