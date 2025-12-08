using Compras.Abstraccion.Repositorios;
using Compras.Abstraccion.Servicios;
using Compras.DTO.EspecializadosDTO;
using ERP.Data.Modelos;

namespace Compras.Implementaciones.Servicios
{
    public class ServicioEspecializado : IServicioEspecializado
    {
        private readonly IRepositorioEspecializado _repositorioEspecializado;
        public ServicioEspecializado(IRepositorioEspecializado repositorioEspecializado)
        {
            _repositorioEspecializado = repositorioEspecializado;
        }

        public async Task<Resultado<object>> ActualizarEstadoOrden(int ordenId, ActualizarEstadoOrdenDTO actualizarEstadoOrdenDTO)
        {
            var orden = await _repositorioEspecializado.ObtenerOrdenPorId(ordenId);
            if (orden == null)
            {
                return Resultado<object>.Falla("Orden no encontrada.");
            }

            orden.EstadoTimelineId = actualizarEstadoOrdenDTO.EstadoTimelineId;
            orden.ActualizadoEn = DateTime.Now;

            _repositorioEspecializado.InsertarTimeline(new OrdenTimeline
            {
                OrdenId = ordenId,
                EstadoTimelineId = actualizarEstadoOrdenDTO.EstadoTimelineId,
                Evento = actualizarEstadoOrdenDTO.Evento ?? "Cambio Manual",
                FechaEvento = DateTime.Now,
                CreadoPor = actualizarEstadoOrdenDTO.UsuarioId
            });

            await _repositorioEspecializado.GuardarCambios();

            return Resultado<object>.Exito(new
            {
                estadoActual = actualizarEstadoOrdenDTO.EstadoTimelineId
            });
        }

        public async Task<Resultado<object>> ActualizarItemRecepcion(int itemId, ActualizarItemRecepcionDTO actualizarItemRecepcionDTO)
        {
            var item = await _repositorioEspecializado.ObtenerItemPorId(itemId);
            if (item == null)
            {
                return Resultado<object>.Falla("Item no encontrado.");
            }

            item.CantidadRecibida = actualizarItemRecepcionDTO.CantidadRecibida;

            if (actualizarItemRecepcionDTO.CantidadRecibida == 0)
            {
                item.EstadoTimelineId = 1; // Registrado
            }
            else if (actualizarItemRecepcionDTO.CantidadRecibida < item.Cantidad)
            {
                item.EstadoTimelineId = 6; // Parcialmente Recibido
            }
            else
            {
                item.EstadoTimelineId = 7; // Recibido 
            }

            await _repositorioEspecializado.GuardarCambios();

            var recalculado = await RecalcularEstadoOrden(item.OrdenId, actualizarItemRecepcionDTO.UsuarioId);

            return Resultado<object>.Exito(new
            {
                item = new
                {
                    item.Id,
                    item.CantidadRecibida,
                    item.EstadoTimelineId
                },
                orden = recalculado.Valor!
            });
        }

        public async Task<Resultado<object>> RecalcularEstadoOrden(int ordenId, int usuarioId)
        {
            var items = await _repositorioEspecializado.ObtenerItemsPorOrden(ordenId);
            var orden = await _repositorioEspecializado.ObtenerOrdenPorId(ordenId);

            if (orden == null)
            {
                return Resultado<object>.Falla("Orden no encontrada.");
            }

            int total = items.Count;
            int recibidos = items.Count(items => items.EstadoTimelineId == 7);
            int parciales = items.Count(items => items.EstadoTimelineId == 6);

            int nuevoEstadoId;

            if (recibidos == total)
            {
                nuevoEstadoId = 8; // Completamente Recibido
            }
            else if (parciales > 0)
            {
                nuevoEstadoId = 6; // Parcialmente Recibido
            }
            else
            {
                nuevoEstadoId = 1; // Registrado
            }

            if (orden.EstadoTimelineId != nuevoEstadoId)
            {
                await ActualizarEstadoOrden(ordenId, new ActualizarEstadoOrdenDTO
                {
                    EstadoTimelineId = nuevoEstadoId,
                    Evento = "Actualización automática por ítems",
                    UsuarioId = usuarioId
                });
            }

            return Resultado<object>.Exito(new
            {
                estadoActual = nuevoEstadoId
            });
        }

        public async Task<Resultado<List<TimelineDTO>>> ObtenerTimeline(int ordenId)
        {
            var timeline = await _repositorioEspecializado.ObtenerTimeline(ordenId);

            var resultado = timeline.Select(t => new TimelineDTO
            {
                FechaEvento = t.FechaEvento,
                Evento = t.Evento,
                EstadoTimelineId = t.EstadoTimelineId,
                Usuario = t.CreadoPorNavigation != null ? $"{t.CreadoPorNavigation.NombreUsuario} {t.CreadoPorNavigation.ApellidoUsuario}" : "Desconocido"
            }).ToList();

            return Resultado<List<TimelineDTO>>.Exito(resultado);
        }

        public async Task<Resultado<List<OrdenItem>>> ObtenerItems(int ordenId)
        {
            var items = await _repositorioEspecializado.ObtenerItemsPorOrden(ordenId);
            return Resultado<List<OrdenItem>>.Exito(items);
        }
    }
}
