using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface ILikeRepositorio
    {
        /// <summary>
        /// Obtiene todos los likes registrados.
        /// </summary>
        Task<List<Like>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un like específico por su Id.
        /// </summary>
        Task<Like?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo like y devuelve true si fue exitoso.
        /// </summary>
        Task<bool> CrearAsync(Like like);

        /// <summary>
        /// Elimina un like por su Id.
        /// </summary>
        Task<bool> EliminarAsync(int id);

        /// <summary>
        /// Cuenta cuántos likes tiene un anuncio específico.
        /// </summary>
        Task<int> ContarPorAnuncioAsync(int anuncioId);

        /// <summary>
        /// Guarda los cambios pendientes en la base de datos.
        /// </summary>
        Task GuardarAsync();
    }
}
