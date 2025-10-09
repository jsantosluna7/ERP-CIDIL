using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    /// <summary>
    /// Interfaz para manejar todas las operaciones relacionadas con los anuncios.
    /// </summary>
    public interface IAnuncioRepositorio
    {
        /// <summary>
        /// Obtiene todos los anuncios junto con sus comentarios y likes.
        /// </summary>
        /// <returns>Lista de anuncios completos.</returns>
        Task<List<Anuncio>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un anuncio específico por su ID.
        /// </summary>
        /// <param name="id">Identificador único del anuncio.</param>
        /// <returns>Objeto Anuncio o null si no existe.</returns>
        Task<Anuncio?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo anuncio en la base de datos.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio a crear.</param>
        Task CrearAsync(Anuncio anuncio);

        /// <summary>
        /// Actualiza los datos de un anuncio existente.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio modificada.</param>
        void Actualizar(Anuncio anuncio);

        /// <summary>
        /// Elimina un anuncio existente de la base de datos.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio a eliminar.</param>
        void Eliminar(Anuncio anuncio);

        /// <summary>
        /// Guarda los cambios realizados en la base de datos.
        /// </summary>
        Task GuardarAsync();
    }
}
