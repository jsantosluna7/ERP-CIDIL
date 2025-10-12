using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
    public interface ILikeServicio
    {
        Task<List<LikeDTO>> ObtenerTodosAsync();
        Task<LikeDTO?> ObtenerPorIdAsync(int id);
        Task<(bool estadoActual, int totalLikes)> CrearAsync(LikeDTO dto); // Toggle Like
        Task<bool> EliminarAsync(int id);
        Task<int> ContarPorAnuncioAsync(int anuncioId);

        // ✅ Nuevo método: verifica si un usuario ya dio like
        Task<bool> ExisteLikeAsync(int anuncioId, string usuario);
    }
}
