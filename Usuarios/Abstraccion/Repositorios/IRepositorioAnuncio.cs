using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    // Interfaz que define los métodos para la gestión de anuncios y currículums. Utiliza <see cref="Resultado{T}"/> para manejo consistente de errores y resultados.
    public interface IRepositorioAnuncio
    {
        // Obtiene todos los anuncios registrados en la base de datos.
        Task<Resultado<List<Anuncio>>> ObtenerTodosAsync();

        // Obtiene un anuncio por su identificador único.
        Task<Resultado<Anuncio>> ObtenerPorIdAsync(int id);

        // Crea un nuevo anuncio en la base de datos.
        Task<Resultado<bool>> CrearAsync(Anuncio anuncio);

        // Actualiza la información de un anuncio existente.
        Task<Resultado<bool>> ActualizarAsync(Anuncio anuncio);

        // Elimina un anuncio de la base de datos por su identificador.
        Task<Resultado<bool>> EliminarAsync(int id);

        // Guarda los cambios pendientes en el contexto de base de datos.
        Task GuardarAsync();

        // Alterna (agrega o quita) un "like" de un usuario sobre un anuncio.
        Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId);

        // Obtiene los comentarios y los likes asociados a un anuncio específico.
        Task<Resultado<(List<Comentario> Comentarios, List<Like> Likes)>> ObtenerComentariosYLikesAsync(int anuncioId);

        // Guarda un currículum asociado a un anuncio (o externo).
        Task<Resultado<bool>> AgregarCurriculumAsync(Curriculum curriculum);

        // Obtiene todos los currículums asociados a un anuncio.
        Task<Resultado<List<Curriculum>>> ObtenerCurriculumsAsync(int anuncioId);
    }
}
