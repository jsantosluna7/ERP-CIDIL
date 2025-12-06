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

        public async Task<Resultado<OrdenTimeline>> CrearOrdenTimeline(CrearOrdenTimelineDTO orden)
        {
            if (orden == null)
            {
                Resultado<OrdenTimeline>.Falla("No se pueden dejar campos vacios.");
            }

            var ordenes = new OrdenTimeline
            {
                Id = orden.OrdenId,
                OrdenId = orden.OrdenId,
                EstadoTimelineId = orden.EstadoTimelineId,
                Evento = orden.Evento,
                FechaEvento = DateTime.Now,
                CreadoPor = orden.CreadoPor,
            };

            _context.OrdenTimelines.Add(ordenes);
            await _context.SaveChangesAsync();
            return Resultado<OrdenTimeline>.Exito(ordenes);
        }

        public async Task<Resultado<OrdenTimeline>> ActualizarOrdenTimeline(int id, CrearOrdenTimelineDTO ordenDTO)
        {
            var existeOrdenTimeline = await OrdenTimelineId(id);
            var ordenTimeline = existeOrdenTimeline.Valor;

            if (ordenTimeline == null)
            {
                return Resultado<OrdenTimeline>.Falla(existeOrdenTimeline.MensajeError);
            }

            var ordenesTimeline = new OrdenTimeline
            {
                Id = ordenDTO.OrdenId,
                OrdenId = ordenDTO.OrdenId,
                EstadoTimelineId = ordenDTO.EstadoTimelineId,
                Evento = ordenDTO.Evento,
                FechaEvento = DateTime.Now,
                CreadoPor = ordenDTO.CreadoPor,
            };

            _context.Update(ordenesTimeline);
            _context.SaveChanges();
            var ordenesTimelineActualizados = await OrdenTimelineId(id);
            var ordenesTimelineAct = ordenesTimelineActualizados.Valor!;
            return Resultado<OrdenTimeline>.Exito(ordenesTimelineAct);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var ordenTimelinePorId = await OrdenTimelineId(id);
            var ordenTimeline = ordenTimelinePorId.Valor!;

            if (ordenTimeline == null)
            {
                return Resultado<bool?>.Falla(ordenTimelinePorId.MensajeError);
            }

            _context.Remove(ordenTimelinePorId);
            _context.SaveChanges();
            return Resultado<bool?>.Exito(true);
        }
    }
}
