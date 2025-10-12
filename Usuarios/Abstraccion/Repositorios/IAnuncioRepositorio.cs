using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    /// <summary>
    /// Interfaz para manejar las operaciones con los anuncios.
    /// </summary>
    public interface IAnuncioRepositorio
    {
        /// <summary>
        /// Obtiene todos los anuncios disponibles.
        /// </summary>
        Task<List<Anuncio>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un anuncio específico por su ID.
        /// </summary>
        Task<Anuncio?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo anuncio en la base de datos.
        /// </summary>
        Task CrearAsync(Anuncio anuncio);

        /// <summary>
        /// Actualiza la información de un anuncio existente.
        /// </summary>
        void Actualizar(Anuncio anuncio);

        /// <summary>
        /// Elimina un anuncio existente.
        /// </summary>
        void Eliminar(Anuncio anuncio);

        /// <summary>
        /// Guarda los cambios realizados en la base de datos.
        /// </summary>
        Task GuardarAsync();

        /// <summary>
        /// Alterna el "like" de un usuario en un anuncio (añadir o quitar).
        /// </summary>
        /// <param name="anuncioId">ID del anuncio</param>
        /// <param name="usuario">Identificador o nombre del usuario</param>
        /// <returns>True si el like fue añadido, False si fue quitado</returns>
        Task<bool> ToggleLikeAsync(int anuncioId, string usuario);
    }
}
