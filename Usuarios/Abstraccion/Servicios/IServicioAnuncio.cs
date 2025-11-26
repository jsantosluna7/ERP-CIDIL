using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioAnuncio
    {
        // Obtiene todos los anuncios filtrando opcionalmente por pasantía o carrusel.
        Task<Resultado<List<AnuncioDetalleDTO>>> ObtenerTodosAsync(bool? esPasantia = null, bool? esCarrusel = null);

        // Obtiene los detalles de un anuncio por su ID.
        Task<Resultado<AnuncioDetalleDTO>> ObtenerPorIdAsync(int id);

        // Crea un nuevo anuncio en la base de datos.
        Task<Resultado<Anuncio>> CrearAsync(Anuncio anuncio);

        // Actualiza un anuncio existente.
        Task<Resultado<bool>> ActualizarAsync(int id, ActualizarAnuncioDTO dto);

        // Elimina un anuncio por su ID.
        Task<Resultado<bool>> EliminarAsync(int id);

        // Obtiene la lista de currículums enviados a un anuncio de pasantía.
        Task<Resultado<List<string>>> ObtenerCurriculumsAsync(int id);

        // Alterna el "like" de un usuario en un anuncio.
        Task<Resultado<bool>> ToggleLikeAsync(int anuncioId, int usuarioId);
    }
}
