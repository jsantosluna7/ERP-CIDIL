using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface ILikeRepositorio
    {
        Task<List<Like>> ObtenerTodosAsync();
        Task<Like?> ObtenerPorIdAsync(int id);
        Task<Like?> ObtenerPorAnuncioYUsuarioAsync(int anuncioId, string usuario);
        Task<bool> CrearAsync(Like like);
        Task<bool> EliminarAsync(int id);
        Task<int> ContarPorAnuncioAsync(int anuncioId);
    }
}
