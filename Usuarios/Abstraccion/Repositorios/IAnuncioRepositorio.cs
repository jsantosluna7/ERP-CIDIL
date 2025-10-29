using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    
    /// Interfaz que define los métodos para la gestión de anuncios.
    /// Utiliza <see cref="Resultado{T}"/> para garantizar un manejo consistente
    /// de errores y resultados a nivel de capa de datos.

    public interface IAnuncioRepositorio
    {
         /// Obtiene todos los anuncios registrados en la base de datos.
        
        
        /// Una lista con todos los anuncios, o un mensaje de error en caso de fallo.
       
        Task<Resultado<List<Anuncio>>> ObtenerTodosAsync();

        
        /// Obtiene un anuncio por su identificador único.
        
        /// <param name="id">ID del anuncio.</param>
        
        Task<Resultado<Anuncio>> ObtenerPorIdAsync(int id);

 
        /// Crea un nuevo anuncio en la base de datos.
   
        /// <param name="anuncio">Entidad del anuncio a crear.</param>
        /// <returns>
        /// True si la creación fue exitosa, o un mensaje de error en caso contrario.
        /// </returns>
        Task<Resultado<bool>> CrearAsync(Anuncio anuncio);

        
        /// Actualiza la información de un anuncio existente.
   
        /// <param name="anuncio">Entidad del anuncio con los cambios a aplicar.</param>
        /// <returns>
        /// True si la actualización fue exitosa, o un mensaje de error si falla.
        /// </returns>
        Task<Resultado<bool>> ActualizarAsync(Anuncio anuncio);

   
        /// Elimina un anuncio de la base de datos por su identificador.
        
        /// <param name="id">ID del anuncio a eliminar.</param>
        /// <returns>
        /// True si la eliminación fue exitosa, o un mensaje de error si no se puede eliminar.
        /// </returns>
        Task<Resultado<bool>> EliminarAsync(int id);

        
        /// Guarda los cambios pendientes en el contexto de base de datos.
      
        Task GuardarAsync();

        
        /// Alterna (agrega o quita) un "like" de un usuario sobre un anuncio.
       
        /// <param name="anuncioId">ID del anuncio.</param>
        /// <param name="usuarioId">ID del usuario.</param>
        /// <returns>
        /// True si el like fue agregado, False si fue eliminado.
        /// </returns>
        Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId);

 
        /// Obtiene los comentarios y los likes asociados a un anuncio específico.
        
        /// <param name="anuncioId">ID del anuncio.</param>
        /// <returns>
        /// Una tupla que contiene la lista de comentarios y la lista de likes del anuncio.
        /// </returns>
        Task<Resultado<(List<Comentario> Comentarios, List<Like> Likes)>> ObtenerComentariosYLikesAsync(int anuncioId);
    }
}
