using Inventario.Abstraccion.Repositorio;
using Inventario.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Implementaciones.Repositorios
{
    public class RepositorioEstadoFisico : IRepositorioEstadoFisico
    {
        private readonly DbErpContext _context;

        public RepositorioEstadoFisico(DbErpContext context)
        {
            _context = context;
        }

        public async Task<EstadoFisico?> GetById(int id)
        {
           return await _context.EstadoFisicos.Include(i=>i.InventarioEquipos).Where( e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<EstadoFisico>?> GetEstadoFisico()
        {
            return await _context.EstadoFisicos.ToListAsync();
        }


    }
}
