using Compras.Abstraccion.Repositorios;
using Compras.Abstraccion.Servicios;
using Compras.DTO.OrdenItemDTO;
using Compras.DTO.OrdenTimelineDTO;
using Compras.Implementaciones.Repositorios;
using ERP.Data.Modelos;

namespace Compras.Implementaciones.Servicios
{
    public class ServicioOrdenTimeline : IServicioOrdenTimeline
    {
        private readonly IRepositorioOrdenTimeline _repositorioOrdenTimeline;
        public ServicioOrdenTimeline(IRepositorioOrdenTimeline repositorioOrdenTimeline)
        {
            _repositorioOrdenTimeline = repositorioOrdenTimeline;
        }

        public async Task<Resultado<List<OrdenTimeline>>> OrdenTimeline()
        {
            var ordenTimelineTodos = await _repositorioOrdenTimeline.OrdenTimeline();
            var ordenTimeline = ordenTimelineTodos.Valor;

            if (ordenTimeline == null || ordenTimeline.Count == 0)
            {
                return Resultado<List<OrdenTimeline>>.Falla(ordenTimelineTodos.MensajeError);
            }

            var ordenTimelinesDTO = new List<OrdenTimeline>();

            foreach (OrdenTimeline orden in ordenTimeline)
            {
                var ordenTimelineDTO = new OrdenTimeline
                {
                    Id = orden.Id,
                    OrdenId = orden.OrdenId,
                    EstadoTimelineId = orden.EstadoTimelineId,
                    Evento = orden.Evento,
                    FechaEvento = orden.FechaEvento,
                    CreadoPor = orden.CreadoPor
                };
                ordenTimelinesDTO.Add(ordenTimelineDTO);
            }
            return Resultado<List<OrdenTimeline>>.Exito(ordenTimelinesDTO);
        }

        public async Task<Resultado<OrdenTimeline>> OrdenTimelineId(int id)
        {
            var resultado = await _repositorioOrdenTimeline.OrdenTimelineId(id);
            var orden = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<OrdenTimeline>.Falla(resultado.MensajeError);
            }

            var ordenDTO = new OrdenTimeline
            {
                Id = orden.Id,
                OrdenId = orden.OrdenId,
                EstadoTimelineId = orden.EstadoTimelineId,
                Evento = orden.Evento,
                FechaEvento = orden.FechaEvento,
                CreadoPor = orden.CreadoPor
            };

            return Resultado<OrdenTimeline>.Exito(ordenDTO);
        }

        public async Task<Resultado<List<OrdenTimeline>>> OrdenTimelinePorOrdenId(int ordenId)
        {
            var resultado = await _repositorioOrdenTimeline.OrdenTimelinePorOrdenId(ordenId);
            var ordenesTimeline = resultado.Valor!;
            if (!resultado.esExitoso)
            {
                return Resultado<List<OrdenTimeline>>.Falla(resultado.MensajeError);
            }
            var ordenTimelineDTO = new List<OrdenTimeline>();
            foreach (var orden in ordenesTimeline)
            {
                var ordenTimelinesDTO = new OrdenTimeline
                {
                    Id = orden.Id,
                    OrdenId = orden.OrdenId,
                    EstadoTimelineId = orden.EstadoTimelineId,
                    Evento = orden.Evento,
                    FechaEvento = orden.FechaEvento,
                    CreadoPor = orden.CreadoPor
                };
                ordenTimelineDTO.Add(ordenTimelinesDTO);
            }
            return Resultado<List<OrdenTimeline>>.Exito(ordenTimelineDTO);
        }
    }
}
