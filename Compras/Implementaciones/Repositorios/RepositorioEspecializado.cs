using Compras.Abstraccion.Repositorios;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Compras.Implementaciones.Repositorios
{
    public class RepositorioEspecializado : IRepositorioEspecializado
    {
        private readonly DbErpContext _context;

        public RepositorioEspecializado(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Ordene?> ObtenerOrdenPorId(int ordenId)
        {
            return await _context.Ordenes.FindAsync(ordenId);
        }

        public async Task<List<OrdenItem>> ObtenerItemsPorOrden(int ordenId)
        {
            return await _context.OrdenItems
                    .Where(item => item.OrdenId == ordenId)
                    .ToListAsync();
        }

        public async Task<OrdenItem?> ObtenerItemPorId(int itemId)
        {
            return await _context.OrdenItems.FindAsync(itemId);
        }

        public async Task<List<OrdenTimeline>> ObtenerTimeline(int ordenId)
        {
            return await _context.OrdenTimelines
                    .Where(t => t.OrdenId == ordenId)
                    .OrderBy(t => t.FechaEvento)
                    .ToListAsync();
        }

        public void InsertarTimeline(OrdenTimeline timeline)
        {
            _context.OrdenTimelines.Add(timeline);
        }

        public async Task GuardarCambios()
        {
            await _context.SaveChangesAsync();
        }
    }
}
