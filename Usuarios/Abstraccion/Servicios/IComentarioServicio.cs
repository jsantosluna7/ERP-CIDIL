using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Abstraccion.Servicios
{
    // Define los métodos de negocio para la gestión de comentarios.
    public interface IComentarioServicio
    {
        // Obtiene todos los comentarios del sistema, incluyendo el nombre del usuario y el título del anuncio.
        Task<Resultado<List<ComentarioDetalleDTO>>> ObtenerTodosAsync();

        // Obtiene un comentario por su ID.
        Task<Resultado<ComentarioDetalleDTO>> ObtenerPorIdAsync(int id);

        // Obtiene todos los comentarios asociados a un anuncio específico.
        Task<Resultado<List<ComentarioDetalleDTO>>> ObtenerPorAnuncioIdAsync(int anuncioId);

        // Crea un nuevo comentario asociado a un anuncio y usuario.
        Task<Resultado<ComentarioDetalleDTO>> CrearAsync(CrearComentarioDTO dto);

        // Actualiza el texto de un comentario existente.
        Task<Resultado<bool>> ActualizarAsync(int id, ActualizarComentarioDTO dto);

        // Elimina un comentario por su ID.
        Task<Resultado<bool>> EliminarAsync(int id);
    }
}
