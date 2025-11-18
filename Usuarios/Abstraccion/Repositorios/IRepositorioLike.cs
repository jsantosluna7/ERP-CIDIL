using ERP.Data.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioLike
    {
       
        /// Obtiene todos los likes.
    
        Task<Resultado<List<Like>>> ObtenerTodosAsync();

   
        /// Obtiene un like por su Id.
 
        Task<Resultado<Like>> ObtenerPorIdAsync(int id);

        
        /// Obtiene un like de un anuncio específico por correo de usuario.
        
        Task<Resultado<Like>> ObtenerPorAnuncioYUsuarioAsync(int anuncioId, string correoUsuario);

        
        /// Crea un nuevo like.
       
        Task<Resultado<bool>> CrearAsync(Like like);

        
        /// Elimina un like por su Id.
        
        Task<Resultado<bool>> EliminarAsync(int id);

        
        /// Cuenta los likes de un anuncio.
        
        Task<Resultado<int>> ContarPorAnuncioAsync(int anuncioId);
    }
}
