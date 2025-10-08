using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;

namespace Usuarios.Abstraccion.Servicios
{
    public interface ILikeServicio
    {
        Task<List<LikeDTO>> ObtenerTodosAsync();
        Task<LikeDTO?> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(LikeDTO dto);
        Task<bool> EliminarAsync(int id);
        Task<int> ContarPorAnuncioAsync(int anuncioId);
    }
}
