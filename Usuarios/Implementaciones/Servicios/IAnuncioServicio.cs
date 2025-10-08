using System.Collections.Generic;
using System.Threading.Tasks;
using Usuarios.DTO.AnuncioDTO;    // DTOs: CrearAnuncioDTO, ActualizarAnuncioDTO, AnuncioDetalleDTO

namespace Usuarios.Implementaciones.Servicios
{
    public interface IAnuncioServicio
    {
        // ✅ Obtener todos los anuncios (devuelve DTOs para el frontend)
        Task<List<AnuncioDetalleDTO>> ObtenerTodosAsync();

        // ✅ Crear un nuevo anuncio usando un DTO
        Task CrearAsync(CrearAnuncioDTO dto);

        // ✅ Actualizar un anuncio existente por ID
        Task<bool> ActualizarAsync(int id, ActualizarAnuncioDTO dto);

        // ✅ Eliminar un anuncio por ID
        Task<bool> EliminarAsync(int id);
    }
}
