using ERP.Data.Modelos;
using Publicaciones.DTO.AnuncioDTO;

namespace Publicaciones.Abstraccion.Repositorios
{
    public interface IRepositorioAnuncio
    {
        Task<List<Anuncio>?> GetAnuncios();
        Task<List<Anuncio>?> GetCarrusel();
        Task<List<Anuncio>?> GetPasantias();
        Task<Anuncio?> GetById(int id);
        Task<Anuncio?> Crear(CrearAnuncioDTO dto, int usuarioId);
        Task<Anuncio?> Actualizar(int id, ActualizarAnuncioDTO dto);
        Task<bool?> Eliminar(int id);
    }
}
