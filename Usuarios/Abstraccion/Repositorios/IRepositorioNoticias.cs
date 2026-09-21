using ERP.Data.Modelos;

namespace Usuarios.Abstraccion.Repositorios
{
    public interface IRepositorioNoticias
    {
        Task<List<Noticia>> ObtenerNoticias();
    }
}