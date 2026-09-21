using Usuarios.DTO.NoticiasDTO;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioNoticias
    {
        Task<List<NoticiasDTO>?> ObtenerNoticias();
    }
}