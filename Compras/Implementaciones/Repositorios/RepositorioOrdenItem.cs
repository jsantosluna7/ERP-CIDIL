using Compras.Abstraccion.Repositorios;
using Compras.DTO.ComentariosOrdenDTO;
using Compras.DTO.OrdenItemDTO;
using ERP.Data.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Compras.Implementaciones.Repositorios
{
    public class RepositorioOrdenItem : IRepositorioOrdenItem
    {
        private readonly DbErpContext _context;

        public RepositorioOrdenItem(DbErpContext context)
        {
            _context = context;
        }

        public async Task<Resultado<List<OrdenItem>>> OrdenItem()
        {
            var resultado = await _context.OrdenItems.ToListAsync();

            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<OrdenItem>>.Falla("No se encontraron items de la orden.");
            }

            return Resultado<List<OrdenItem>>.Exito(resultado);
        }

        public async Task<Resultado<OrdenItem>> OrdenItemId(int id)
        {
            var resultado = await _context.OrdenItems.FirstOrDefaultAsync(e => e.Id == id);

            if (resultado == null)
            {
                return Resultado<OrdenItem>.Falla("No se encontró un orden de una orden con el ID");
            }

            return Resultado<OrdenItem>.Exito(resultado);
        }

        public async Task<Resultado<List<OrdenItem>>> OrdenItemPorOrdenId(int ordenId)
        {
            if (ordenId <= 0)
            {
                return Resultado<List<OrdenItem>>.Falla("El ID del item de la orden proporcionado no es válido.");
            }

            var resultado = await _context.OrdenItems
                .Where(e => e.OrdenId == ordenId)
                .ToListAsync();
            if (resultado == null || resultado.Count == 0)
            {
                return Resultado<List<OrdenItem>>.Falla("No se encontraron items de la orden proporcionada.");
            }
            return Resultado<List<OrdenItem>>.Exito(resultado);
        }

        public async Task<Resultado<OrdenItem>> CrearOrdenItem(CrearOrdenItemDTO orden)
        {
            if (orden == null)
            {
                Resultado<OrdenItem>.Falla("No se pueden dejar campos vacios.");
            }

            var ordenes = new OrdenItem
            {
                OrdenId = orden.OrdenId,
                Nombre = orden.Nombre,
                NumeroLista = orden.NumeroLista,
                EstadoTimelineId = orden.EstadoTimelineId,
                Cantidad = orden.Cantidad,
                CantidadRecibida = orden.CantidadRecibida,
                UnidadMedida = orden.UnidadMedida,
                PrecioUnitario = orden.PrecioUnitario,
                ImporteLinea = orden.ImporteLinea,
                LinkExterno = orden.LinkExterno,
                Comentario = orden.Comentario,
                DireccionEnvio = orden.DireccionEnvio,
                Atencion = orden.Atencion,
                EnvioVia = orden.EnvioVia,
                TerminosEnvio = orden.TerminosEnvio
            };

            _context.OrdenItems.Add(ordenes);
            await _context.SaveChangesAsync();
            return Resultado<OrdenItem>.Exito(ordenes);
        }

        public async Task<Resultado<OrdenItem>> ActualizarOrdenItem(int id, CrearOrdenItemDTO ordenDTO)
        {
            var existeOrdenItem = await OrdenItemId(id);
            var ordenItem = existeOrdenItem.Valor;

            if (ordenItem == null)
            {
                return Resultado<OrdenItem>.Falla(existeOrdenItem.MensajeError);
            }

            var ordenesItem = new OrdenItem
            {
                OrdenId = ordenDTO.OrdenId,
                Nombre = ordenDTO.Nombre,
                NumeroLista = ordenDTO.NumeroLista,
                EstadoTimelineId = ordenDTO.EstadoTimelineId,
                Cantidad = ordenDTO.Cantidad,
                CantidadRecibida = ordenDTO.CantidadRecibida,
                UnidadMedida = ordenDTO.UnidadMedida,
                PrecioUnitario = ordenDTO.PrecioUnitario,
                ImporteLinea = ordenDTO.ImporteLinea,
                LinkExterno = ordenDTO.LinkExterno,
                Comentario = ordenDTO.Comentario,
                DireccionEnvio = ordenDTO.DireccionEnvio,
                Atencion = ordenDTO.Atencion,
                EnvioVia = ordenDTO.EnvioVia,
                TerminosEnvio = ordenDTO.TerminosEnvio
            };

            _context.Update(ordenesItem);
            _context.SaveChanges();
            var ordenesItemsActualizados = await OrdenItemId(id);
            var ordenesItemsAct = ordenesItemsActualizados.Valor!;
            return Resultado<OrdenItem>.Exito(ordenesItemsAct);
        }

        public async Task<Resultado<bool?>> Eliminar(int id)
        {
            var ordenItemPorId = await OrdenItemId(id);
            var ordenItem = ordenItemPorId.Valor!;

            if (ordenItem == null)
            {
                return Resultado<bool?>.Falla(ordenItemPorId.MensajeError);
            }

            _context.Remove(ordenItemPorId);
            _context.SaveChanges();
            return Resultado<bool?>.Exito(true);
        }
    }
}
