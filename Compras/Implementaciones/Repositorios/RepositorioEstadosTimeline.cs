using Compras.Abstraccion.Repositorios;
using Compras.DTO.EstadosTimelineDTO;
using Compras.DTO.OrdenesDTO;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Compras.Implementaciones.Repositorios
{
    public class RepositorioEstadosTimeline : IRepositorioEstadosTimeline
    {
        private readonly DbErpContext _context;

        public RepositorioEstadosTimeline(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Resultado<List<EstadosTimeline>>> EstadosTimeline()
        {
            var resultado = await _context.EstadosTimelines.ToListAsync();

            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<EstadosTimeline>>.Falla("No se encontraron estados para el timeline");
            }

            return Resultado<List<EstadosTimeline>>.Exito(resultado);
        }

        public async Task<Resultado<EstadosTimeline>> EstadosTimelineId(int id)
        {
            var resultado = await _context.EstadosTimelines.FirstOrDefaultAsync(e => e.Id == id);

            if (resultado == null)
            {
                return Resultado<EstadosTimeline>.Falla("No se encontró un estado con el ID");
            }

            return Resultado<EstadosTimeline>.Exito(resultado);
        }

        public async Task<Resultado<EstadosTimeline>> CrearEstadosTimeline(EstadosTimelineDTO estado)
        {
            if (estado == null)
            {
                Resultado<EstadosTimeline>.Falla("No se pueden dejar campos vacios.");
            }

            var estados = new EstadosTimeline
            {
                Codigo = estado.Codigo,
                Nombre = estado.Nombre,
                Color = estado.Color,
                Icono = estado.Icono,
                Activo = estado.Activo
            };

            _context.EstadosTimelines.Add(estados);
            await _context.SaveChangesAsync();
            return Resultado<EstadosTimeline>.Exito(estados);
        }

        public async Task<Resultado<EstadosTimeline>> ActualizarEstadosTimeline(int id, EstadosTimelineDTO estadoDTO)
        {
            var existeEstado = await EstadosTimelineId(id);
            var estado = existeEstado.Valor;

            if (estado == null)
            {
                return Resultado<EstadosTimeline>.Falla(existeEstado.MensajeError);
            }

            var estados = new EstadosTimeline
            {
                Codigo = estadoDTO.Codigo,
                Nombre = estadoDTO.Nombre,
                Color = estadoDTO.Color,
                Icono = estadoDTO.Icono,
                Activo = estadoDTO.Activo
            };

            _context.Update(estados);
            _context.SaveChanges();
            var estadosActualizados = await EstadosTimelineId(id);
            var estadosAct = estadosActualizados.Valor!;
            return Resultado<EstadosTimeline>.Exito(estadosAct);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var estadosPorId = await EstadosTimelineId(id);
            var ordenes = estadosPorId.Valor!;

            if (ordenes == null)
            {
                return Resultado<bool?>.Falla(estadosPorId.MensajeError);
            }

            _context.Remove(estadosPorId);
            _context.SaveChanges();
            return Resultado<bool?>.Exito(true);
        }
    }
}
