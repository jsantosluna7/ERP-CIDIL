using ERP.Data.Modelos;
using Publicaciones.DTO.AnuncioDTO;

namespace Publicaciones.Abstraccion.Servicios
{
    public interface IServicioAnuncio
    {
        Task<List<AnuncioDTO>?> GetAnuncios();
        Task<List<AnuncioDTO>?> GetCarrusel();
        Task<List<AnuncioDTO>?> GetPasantias();
        Task<AnuncioDTO?> GetById(int id);
        Task<AnuncioDTO?> Crear(CrearAnuncioDTO dto, int usuarioId);
        Task<AnuncioDTO?> Actualizar(int id, ActualizarAnuncioDTO dto);
        Task<bool?> Eliminar(int id);
    }
}
