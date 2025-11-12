using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
    // Define las operaciones disponibles para gestionar los anuncios dentro del sistema. Todos los métodos devuelven Resultado<T> para manejar errores y mensajes.
    public interface IAnuncioServicio
    {
        // Obtiene la lista de todos los anuncios, con opción de filtrar por tipo de pasantía.
        Task<Resultado<List<AnuncioDetalleDTO>>> ObtenerTodosAsync(bool? esPasantia = null);

        // Obtiene los detalles de un anuncio por su ID.
        Task<Resultado<AnuncioDetalleDTO>> ObtenerPorIdAsync(int id);

        // Crea un nuevo anuncio en la base de datos.
        // 🔥 CORRECCIÓN CLAVE: Se cambia el tipo de retorno de bool a Anuncio
        Task<Resultado<Anuncio>> CrearAsync(Anuncio anuncio);

        // Actualiza un anuncio existente.
        Task<Resultado<bool>> ActualizarAsync(int id, ActualizarAnuncioDTO dto);

        // Elimina un anuncio por su ID.
        Task<Resultado<bool>> EliminarAsync(int id);

        // Obtiene la lista de currículums asociados a un anuncio de pasantía.
        Task<Resultado<List<string>>> ObtenerCurriculumsAsync(int id);

        // Alterna (agrega o quita) el "like" de un usuario en un anuncio.
        Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId);
    }
}