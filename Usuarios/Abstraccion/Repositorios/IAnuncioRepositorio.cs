using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    /// <summary>
    /// Interfaz que define los métodos para la gestión de anuncios y currículums.
    /// Utiliza <see cref="Resultado{T}"/> para manejo consistente de errores y resultados.
    /// </summary>
    public interface IAnuncioRepositorio
    {
        /// <summary>
        /// Obtiene todos los anuncios registrados en la base de datos.
        /// </summary>
        Task<Resultado<List<Anuncio>>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un anuncio por su identificador único.
        /// </summary>
        /// <param name="id">ID del anuncio.</param>
        Task<Resultado<Anuncio>> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo anuncio en la base de datos.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio a crear.</param>
        Task<Resultado<bool>> CrearAsync(Anuncio anuncio);

        /// <summary>
        /// Actualiza la información de un anuncio existente.
        /// </summary>
        /// <param name="anuncio">Entidad del anuncio con los cambios a aplicar.</param>
        Task<Resultado<bool>> ActualizarAsync(Anuncio anuncio);

        /// <summary>
        /// Elimina un anuncio de la base de datos por su identificador.
        /// </summary>
        /// <param name="id">ID del anuncio a eliminar.</param>
        Task<Resultado<bool>> EliminarAsync(int id);

        /// <summary>
        /// Guarda los cambios pendientes en el contexto de base de datos.
        /// </summary>
        Task GuardarAsync();

        /// <summary>
        /// Alterna (agrega o quita) un "like" de un usuario sobre un anuncio.
        /// </summary>
        /// <param name="anuncioId">ID del anuncio.</param>
        /// <param name="usuarioId">ID del usuario.</param>
        Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId);

        /// <summary>
        /// Obtiene los comentarios y los likes asociados a un anuncio específico.
        /// </summary>
        /// <param name="anuncioId">ID del anuncio.</param>
        Task<Resultado<(List<Comentario> Comentarios, List<Like> Likes)>> ObtenerComentariosYLikesAsync(int anuncioId);

        // ===========================
        // Métodos para manejar currículums
        // ===========================

        /// <summary>
        /// Guarda un currículum asociado a un anuncio (o externo).
        /// </summary>
        /// <param name="curriculum">Entidad del currículum a guardar.</param>
        Task<Resultado<bool>> AgregarCurriculumAsync(Curriculum curriculum);

        /// <summary>
        /// Obtiene todos los currículums asociados a un anuncio.
        /// </summary>
        /// <param name="anuncioId">ID del anuncio.</param>
        Task<Resultado<List<Curriculum>>> ObtenerCurriculumsAsync(int anuncioId);
    }
}
