using Compras.Abstraccion.Repositorios;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
                    .OrderBy(item => item.Id)
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

        public async Task<int> CantidadDeOrdenes()
        {
            return await _context.Ordenes.CountAsync();
        }

        public async Task<Resultado<List<Ordene>>> BuscarOrdenes(string termino, string filtro)
        {
            if (string.IsNullOrEmpty(termino))
            {
                return Resultado<List<Ordene>>.Falla("El campo de búsqueda no debe estar vacío.");
            }

            if (string.IsNullOrEmpty(filtro))
            {
                return Resultado<List<Ordene>>.Falla("Debe dar un tipo de filtro, no debe estar vacío.");
            }

            IQueryable<Ordene> query = _context.Ordenes;

            switch (filtro.ToLower())
            {
                case "codigo":
                    query = query.Where(e => e.Codigo != null && EF.Functions.ILike(e.Codigo, $"%{termino}%"));
                    break;
                case "nombre":
                    query = query.Where(e => e.Nombre != null && EF.Functions.ILike(e.Nombre, $"%{termino}%"));
                    break;
            }

            var resultado = await query.ToListAsync();
            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<Ordene>>.Falla("No se encontraron ordenes con el termino y/o filtro seleccionado.");
            }
            return Resultado<List<Ordene>>.Exito(resultado);
        }
    }
}
