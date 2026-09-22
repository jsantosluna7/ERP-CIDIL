using ERP.Data.Modelos;

namespace Usuarios.Abstraccion.Servicios
{
    public interface IServicioNoticias
    {
        Task<List<Noticia>?> ObtenerNoticias();
    }
}