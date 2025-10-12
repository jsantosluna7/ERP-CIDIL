using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;
using Usuarios.DTO.Comentarios;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IComentarioServicio
    {
        Task<List<ComentarioDetalleDTO>> ObtenerTodosAsync();
        Task<ComentarioDetalleDTO?> ObtenerPorIdAsync(int id);
        Task<List<ComentarioDetalleDTO>> ObtenerPorAnuncioIdAsync(int anuncioId);
        Task<ComentarioDetalleDTO> CrearAsync(CrearComentarioDTO dto); // ✅ devuelve el comentario creado
        Task<bool> ActualizarAsync(int id, ActualizarComentarioDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
