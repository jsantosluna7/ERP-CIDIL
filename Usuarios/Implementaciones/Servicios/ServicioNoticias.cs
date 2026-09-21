using ERP.Data.Modelos;
using Usuarios.Abstraccion.Repositorios;
using Usuarios.Abstraccion.Servicios;
using Usuarios.DTO.NoticiasDTO;

namespace Usuarios.Implementaciones.Servicios
{
    public class ServicioNoticias : IServicioNoticias
    {
        private readonly IRepositorioNoticias _repositorioNoticias;
        public ServicioNoticias(IRepositorioNoticias repositorioNoticias)
        {
            _repositorioNoticias = repositorioNoticias;
        }
        // Método para obtener todas las noticias
        public async Task<List<NoticiasDTO>?> ObtenerNoticias()
        {
            // Obtener todas las noticias
            var noticias = await _repositorioNoticias.ObtenerNoticias();
            if (noticias == null || noticias.Count == 0)
            {
                return null;
            }
            //Inicializar la lista de noticias DTO
            var noticiasDTO = new List<NoticiasDTO>();
            // Mapear las noticias a DTOs
            foreach (Noticia noticia in noticias)
            {
                var noticiaDTO = new NoticiasDTO
                {
                    Titulo = noticia.Titulo,
                    Descripcion = noticia.Descripcion,
                    Categoria = noticia.Categoria,
                    CreadoEn = noticia.CreadoEn,
                    Fecha = noticia.Fecha,
                    Foto = noticia.Foto,
                    Propio = noticia.Propio
                };
                noticiasDTO.Add(noticiaDTO);
            }
            // Devolver la lista de noticias DTO
            return noticiasDTO;
        }
    }
}
