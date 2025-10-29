using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IComentarioRepositorio
    {
        Task<Resultado<List<Comentario>>> ObtenerTodosAsync();
        Task<Resultado<Comentario>> ObtenerPorIdAsync(int id);
        Task<Resultado<List<Comentario>>> ObtenerPorAnuncioAsync(int anuncioId);
        Task<Resultado<Comentario>> CrearAsync(Comentario comentario);
        Task<Resultado<bool>> ActualizarAsync(Comentario comentario);
        Task<Resultado<bool>> EliminarPorIdAsync(int id);

        //método para consultas personalizadas (sin cambio)
        IQueryable<Comentario> ObtenerQueryable();
    }
}
