using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Usuarios.Abstraccion.Repositorios;

namespace Usuarios.Implementaciones.Repositorios
{
    public class RepositorioNoticias : IRepositorioNoticias
    {
        private readonly DbErpContext _context;

        public RepositorioNoticias(DbErpContext context)
        {
            _context = context;
        }

        //Método para obtener todas las noticias
        public async Task<List<Noticia>> ObtenerNoticias()
        {
            return await _context.Noticias
                .OrderByDescending(i => i.Id)
                .ToListAsync();
        }
    }
}
