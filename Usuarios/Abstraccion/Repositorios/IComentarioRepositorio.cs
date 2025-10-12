using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IComentarioRepositorio
    {
        Task<List<Comentario>> ObtenerTodosAsync();
        Task<Comentario?> ObtenerPorIdAsync(int id);
        Task<List<Comentario>> ObtenerPorAnuncioAsync(int anuncioId);
        Task CrearAsync(Comentario comentario);
        Task ActualizarAsync(Comentario comentario);
        Task<bool> EliminarPorIdAsync(int id);
        Task GuardarAsync();
    }
}
