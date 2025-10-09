using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
    /// <summary>
    /// Define las operaciones disponibles para la gestión de anuncios.
    /// Incluye métodos para crear, actualizar, eliminar y obtener anuncios.
    /// </summary>
    public interface IAnuncioServicio
    {
        /// <summary>
        /// Obtiene una lista de todos los anuncios con sus detalles,
        /// incluyendo comentarios y cantidad de likes.
        /// </summary>
        /// <returns>Lista de objetos <see cref="AnuncioDetalleDTO"/>.</returns>
        Task<List<AnuncioDetalleDTO>> ObtenerTodosAsync();

        /// <summary>
        /// Crea un nuevo anuncio a partir de un DTO que incluye la imagen.
        /// </summary>
        /// <param name="dto">Datos necesarios para crear el anuncio.</param>
        Task CrearAsync(CrearAnuncioDTO dto);

        /// <summary>
        /// Actualiza un anuncio existente.
        /// </summary>
        /// <param name="id">ID del anuncio a actualizar.</param>
        /// <param name="dto">Datos actualizados del anuncio.</param>
        /// <returns>True si se actualizó correctamente, false en caso contrario.</returns>
        Task<bool> ActualizarAsync(int id, ActualizarAnuncioDTO dto);

        /// <summary>
        /// Elimina un anuncio por su ID.
        /// </summary>
        /// <param name="id">ID del anuncio a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
        Task<bool> EliminarAsync(int id);
    }
}

