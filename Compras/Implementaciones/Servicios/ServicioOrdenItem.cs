using Compras.Abstraccion.Repositorios;
using Compras.Abstraccion.Servicios;
using Compras.DTO.ComentariosOrdenDTO;
using Compras.DTO.OrdenItemDTO;
using ERP.Data.Modelos;

namespace Compras.Implementaciones.Servicios
{
    public class ServicioOrdenItem : IServicioOrdenItem
    {
        private readonly IRepositorioOrdenItem _repositorioOrdenItem;
        public ServicioOrdenItem(IRepositorioOrdenItem repositorioOrdenItem)
        {
            _repositorioOrdenItem = repositorioOrdenItem;
        }

        public async Task<Resultado<List<OrdenItem>>> OrdenItem()
        {
            var ordenItemsTodos = await _repositorioOrdenItem.OrdenItem();
            var ordenItems = ordenItemsTodos.Valor;

            if (ordenItems == null || ordenItems.Count == 0)
            {
                return Resultado<List<OrdenItem>>.Falla(ordenItemsTodos.MensajeError);
            }

            var ordenItemsDTO = new List<OrdenItem>();

            foreach (OrdenItem orden in ordenItems)
            {
                var ordenItemDTO = new OrdenItem
                {
                    Id = orden.Id,
                    OrdenId = orden.OrdenId,
                    Nombre = orden.Nombre,
                    NumeroLista = orden.NumeroLista,
                    EstadoTimelineId = orden.EstadoTimelineId,
                    Cantidad = orden.Cantidad,
                    CantidadRecibida = orden.CantidadRecibida,
                    Comentario = orden.Comentario,
                    ActualizadoEn = orden.ActualizadoEn,
                };
                ordenItemsDTO.Add(ordenItemDTO);
            }
            return Resultado<List<OrdenItem>>.Exito(ordenItemsDTO);
        }

        public async Task<Resultado<OrdenItem>> OrdenItemId(int id)
        {
            var resultado = await _repositorioOrdenItem.OrdenItemId(id);
            var orden = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<OrdenItem>.Falla(resultado.MensajeError);
            }

            var ordenDTO = new OrdenItem
            {
                Id = orden.Id,
                OrdenId = orden.OrdenId,
                Nombre = orden.Nombre,
                NumeroLista = orden.NumeroLista,
                EstadoTimelineId = orden.EstadoTimelineId,
                Cantidad = orden.Cantidad,
                CantidadRecibida = orden.CantidadRecibida,
                Comentario = orden.Comentario,
                ActualizadoEn = orden.ActualizadoEn,
            };

            return Resultado<OrdenItem>.Exito(ordenDTO);
        }

        public async Task<Resultado<List<OrdenItem>>> OrdenItemPorOrdenId(int ordenId)
        {
            var resultado = await _repositorioOrdenItem.OrdenItemPorOrdenId(ordenId);
            var ordenesItem = resultado.Valor!;
            if (!resultado.esExitoso)
            {
                return Resultado<List<OrdenItem>>.Falla(resultado.MensajeError);
            }
            var ordenItemDTO = new List<OrdenItem>();
            foreach (var orden in ordenesItem)
            {
                var ordenItemsDTO = new OrdenItem
                {
                    Id = orden.Id,
                    OrdenId = orden.OrdenId,
                    Nombre = orden.Nombre,
                    NumeroLista = orden.NumeroLista,
                    EstadoTimelineId = orden.EstadoTimelineId,
                    Cantidad = orden.Cantidad,
                    CantidadRecibida = orden.CantidadRecibida,
                    Comentario = orden.Comentario,
                    ActualizadoEn = orden.ActualizadoEn,
                };
                ordenItemDTO.Add(ordenItemsDTO);
            }
            return Resultado<List<OrdenItem>>.Exito(ordenItemDTO);
        }

        public async Task<Resultado<OrdenItemDTO>> CrearOrdenItem(CrearOrdenItemDTO itemsOrden)
        {
            var resultado = await _repositorioOrdenItem.CrearOrdenItem(itemsOrden);
            var orden = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<OrdenItemDTO>.Falla(resultado.MensajeError);
            }

            var ordenDTO = new OrdenItemDTO
            {
                OrdenId = orden.OrdenId,
                Nombre = orden.Nombre,
                NumeroLista = orden.NumeroLista,
                EstadoTimelineId = orden.EstadoTimelineId,
                Cantidad = orden.Cantidad,
                CantidadRecibida = orden.CantidadRecibida,
                Comentario = orden.Comentario,
                ActualizadoEn = orden.ActualizadoEn,
            };

            return Resultado<OrdenItemDTO>.Exito(ordenDTO);
        }

        public async Task<Resultado<OrdenItemDTO>> ActualizarOrdenItem(int id, CrearOrdenItemDTO ordenItemsDTO)
        {
            var resultado = await _repositorioOrdenItem.ActualizarOrdenItem(id, ordenItemsDTO);
            var orden = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<OrdenItemDTO>.Falla(resultado.MensajeError);
            }

            var ordenDTO = new OrdenItemDTO
            {
                OrdenId = orden.OrdenId,
                Nombre = orden.Nombre,
                NumeroLista = orden.NumeroLista,
                EstadoTimelineId = orden.EstadoTimelineId,
                Cantidad = orden.Cantidad,
                CantidadRecibida = orden.CantidadRecibida,
                Comentario = orden.Comentario,
                ActualizadoEn = orden.ActualizadoEn,
            };

            return Resultado<OrdenItemDTO>.Exito(ordenDTO);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var resultado = await _repositorioOrdenItem.Eliminar(id);
            var estado = resultado.Valor!;

            if (!resultado.esExitoso)
            {
                return Resultado<bool?>.Falla(resultado.MensajeError);
            }

            return Resultado<bool?>.Exito(estado);
        }
    }
}
