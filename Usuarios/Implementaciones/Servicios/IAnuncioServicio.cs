using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
    /// <summary>
    /// Define las operaciones disponibles para gestionar los anuncios dentro del sistema.
    /// </summary>
    public interface IAnuncioServicio
    {
        /// <summary>
        /// Obtiene la lista de todos los anuncios, con opción de filtrar por tipo de pasantía.
        /// </summary>
        /// <param name="esPasantia">Si se especifica, filtra los anuncios que sean o no pasantías.</param>
        Task<List<AnuncioDetalleDTO>> ObtenerTodosAsync(bool? esPasantia = null);

        /// <summary>
        /// Obtiene los detalles de un anuncio por su ID.
        /// </summary>
        /// <param name="id">ID del anuncio.</param>
        Task<AnuncioDetalleDTO?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo anuncio en la base de datos.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio a crear.</param>
        Task CrearAsync(Anuncio anuncio);

        /// <summary>
        /// Actualiza un anuncio existente.
        /// </summary>
        /// <param name="id">ID del anuncio a actualizar.</param>
        /// <param name="dto">Datos actualizados del anuncio.</param>
        Task<bool> ActualizarAsync(int id, ActualizarAnuncioDTO dto);

        /// <summary>
        /// Elimina un anuncio por su ID.
        /// </summary>
        /// <param name="id">ID del anuncio a eliminar.</param>
        Task<bool> EliminarAsync(int id);

        /// <summary>
        /// Obtiene la lista de currículums asociados a un anuncio de pasantía.
        /// </summary>
        /// <param name="id">ID del anuncio de pasantía.</param>
        Task<List<string>> ObtenerCurriculumsAsync(int id);

        /// <summary>
        /// Alterna (agrega o quita) el "like" de un usuario en un anuncio.
        /// </summary>
        /// <param name="anuncioId">ID del anuncio.</param>
        /// <param name="usuarioId">ID del usuario que da o quita el "like".</param>
        /// <returns>Devuelve true si la operación se realiza correctamente.</returns>
        Task<bool> ToggleLikeAsync(int anuncioId, int usuarioId);
    }
}
