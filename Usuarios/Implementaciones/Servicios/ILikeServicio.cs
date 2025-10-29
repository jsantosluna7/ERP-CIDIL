using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
    /// <summary>
    /// Interfaz del servicio de Likes para anuncios.
    /// Define las operaciones disponibles sobre Likes.
    /// </summary>
    public interface ILikeServicio
    {
        Task<List<LikeDTO>> ObtenerTodosAsync();
        Task<LikeDTO?> ObtenerPorIdAsync(int id);
        Task<(bool estadoActual, int totalLikes)> CrearAsync(LikeDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<int> ContarPorAnuncioAsync(int anuncioId);
        Task<bool> ExisteLikeAsync(int anuncioId, string usuarioCorreo);
    }
}
