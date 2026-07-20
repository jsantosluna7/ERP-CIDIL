using Compras.Abstraccion.Repositorios;
using Compras.DTO.OrdenItemDTO;
using Compras.DTO.OrdenTimelineDTO;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Compras.Implementaciones.Repositorios
{
    public class RepositorioOrdenTimeline : IRepositorioOrdenTimeline
    {
        private readonly DbErpContext _context;

        public RepositorioOrdenTimeline(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Resultado<List<OrdenTimeline>>> OrdenTimeline()
        {
            var resultado = await _context.OrdenTimelines.ToListAsync();

            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<OrdenTimeline>>.Falla("No se encontraron timelines de la orden.");
            }

            return Resultado<List<OrdenTimeline>>.Exito(resultado);
        }

        public async Task<Resultado<OrdenTimeline>> OrdenTimelineId(int id)
        {
            var resultado = await _context.OrdenTimelines.FirstOrDefaultAsync(e => e.Id == id);

            if (resultado == null)
            {
                return Resultado<OrdenTimeline>.Falla("No se encontró un timeline de una orden con el ID");
            }

            return Resultado<OrdenTimeline>.Exito(resultado);
        }

        public async Task<Resultado<List<OrdenTimeline>>> OrdenTimelinePorOrdenId(int ordenId)
        {
            if (ordenId <= 0)
            {
                return Resultado<List<OrdenTimeline>>.Falla("El ID de la orden proporcionado no es válido.");
            }

            var resultado = await _context.OrdenTimelines
                .Where(e => e.OrdenId == ordenId)
                .ToListAsync();
            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<OrdenTimeline>>.Falla("No se encontraron timeline de la orden proporcionada.");
            }
            return Resultado<List<OrdenTimeline>>.Exito(resultado);
        }
    }
}
